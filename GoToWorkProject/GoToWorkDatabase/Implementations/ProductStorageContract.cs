using AutoMapper;
using GoToWorkContracts.DataModels;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.StoragesContracts;
using GoToWorkDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace GoToWorkDatabase.Implementations;

internal class ProductStorageContract : IProductStorageContract
{
    private readonly GoToWorkDbContext _dbContext;
    private readonly Mapper _mapper;

    public ProductStorageContract(GoToWorkDbContext dbContext)
    {
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Product, ProductDataModel>()
                .ForMember(dest => dest.Details, opt => opt.MapFrom(x => x.DetailProducts));
            cfg.CreateMap<ProductDataModel, Product>()
                .ForMember(dest => dest.DetailProducts, opt => opt.MapFrom(x => x.Details));
            cfg.CreateMap<Machine, MachineDataModel>();
            cfg.CreateMap<MachineDataModel, Machine>();
            cfg.CreateMap<Detail, DetailDataModel>();
            cfg.CreateMap<DetailDataModel, Detail>();
            cfg.CreateMap<DetailProduct, DetailProductDataModel>();
            cfg.CreateMap<DetailProductDataModel, DetailProduct>();
        });
        _mapper = new Mapper(config);
    }

    public List<ProductDataModel> GetList(DateTime? startDate = null, DateTime? endDate = null,
        string? machineId = null)
    {
        try
        {
            var query = _dbContext.Products
                .Include(x => x.Machine)
                .Include(x => x.DetailProducts)!
                .ThenInclude(x => x.Detail)
                .AsQueryable();
            if (startDate is not null)
                query = query.Where(p => p.CreationDate >= startDate.Value);
            if (endDate is not null)
                query = query.Where(p => p.CreationDate <= endDate.Value);
            if (machineId is not null)
                query = query.Where(p => p.MachineId == machineId);
            return [..query.Select(p => _mapper.Map<ProductDataModel>(p))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public ProductDataModel? GetElementById(string id)
    {
        try
        {
            return _mapper.Map<ProductDataModel>(GetProductById(id));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public ProductDataModel? GetElementByName(string name)
    {
        try
        {
            return _mapper.Map<ProductDataModel>(_dbContext.Products
                .Include(x => x.Machine)
                .Include(x => x.DetailProducts)!
                .ThenInclude(x => x.Detail)
                .FirstOrDefault(p => p.Name == name));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void AddElement(ProductDataModel productDataModel)
    {
        try
        {
            _dbContext.Products.Add(_mapper.Map<Product>(productDataModel));
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex) when (ex.TargetSite?.Name == "ThrowIdentityConflict")
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", productDataModel.Id);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void UpdElement(ProductDataModel productDataModel)
    {
        try
        {
            var existingProduct = GetProductById(productDataModel.Id)
                                  ?? throw new ElementNotFoundException(productDataModel.Id);
            _dbContext.Products.Update(_mapper.Map(productDataModel, existingProduct));
            _dbContext.SaveChanges();
        }
        catch (Exception ex) when (ex is ElementNotFoundException)
        {
            _dbContext.ChangeTracker.Clear();
            throw;
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void DelElement(string id)
    {
        try
        {
            var product = GetProductById(id) ?? throw new ElementNotFoundException(id);
            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();
        }
        catch (Exception ex) when (ex is ElementNotFoundException)
        {
            _dbContext.ChangeTracker.Clear();
            throw;
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    private Product? GetProductById(string id) => _dbContext.Products
        .Include(x => x.Machine)
        .Include(x => x.DetailProducts)!
        .ThenInclude(x => x.Detail)
        .FirstOrDefault(p => p.Id == id);
}