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
            cfg.CreateMap<Product, ProductDataModel>();
            cfg.CreateMap<ProductDataModel, Product>();
            cfg.CreateMap<DetailProduct, DetailProductDataModel>();
            cfg.CreateMap<DetailProductDataModel, DetailProduct>();
        });
        _mapper = new Mapper(config);
    }

    public List<ProductDataModel> GetList(DateTime? startDate = null, DateTime? endDate = null,
        string? machineId = null, string? detailId = null)
    {
        try
        {
            var query = _dbContext.Products
                .Include(p => p.Machine)
                .Include(p => p.Details)!
                .ThenInclude(d => d.Detail)
                .AsQueryable();

            if (startDate is not null)
                query = query.Where(p => p.CreationDate >= startDate.Value);

            if (endDate is not null)
                query = query.Where(p => p.CreationDate <= endDate.Value);

            if (machineId is not null)
                query = query.Where(p => p.MachineId == machineId);

            if (detailId is not null)
                query = query.Where(p => p.Details!.Any(d => d.DetailId == detailId));

            return query
                .Select(p => _mapper.Map<ProductDataModel>(p))
                .ToList();
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
            var product = GetProductById(id);
            return product != null ? _mapper.Map<ProductDataModel>(product) : null;
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
            var product = _dbContext.Products
                .Include(p => p.Machine)
                .Include(p => p.Details)!
                .ThenInclude(d => d.Detail)
                .FirstOrDefault(p => p.Name == name);

            return product != null ? _mapper.Map<ProductDataModel>(product) : null;
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
            var product = _mapper.Map<Product>(productDataModel);
            _dbContext.Products.Add(product);
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

            _mapper.Map(productDataModel, existingProduct);
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

    public void AddDetailToProduct(string id, DetailProductDataModel detail)
    {
        try
        {
            var product = GetProductById(id) ?? throw new ElementNotFoundException(id);
            product.Details!.Add(_mapper.Map<DetailProduct>(detail));
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

    public void DelDetailFromProduct(string id, string detailId)
    {
        try
        {
            var product = GetProductById(id) ?? throw new ElementNotFoundException(id);
            product.Details!
                .Remove(product.Details.FirstOrDefault(y => y.DetailId == detailId)
                        ?? throw new ElementNotFoundException(detailId));
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
        .Include(p => p.Machine)
        .Include(p => p.Details)!
        .ThenInclude(d => d.Detail)
        .FirstOrDefault(p => p.Id == id);
}