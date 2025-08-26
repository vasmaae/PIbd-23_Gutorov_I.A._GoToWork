using AutoMapper;
using GoToWorkContracts.DataModels;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.StoragesContracts;
using GoToWorkDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace GoToWorkDatabase.Implementations;

internal class ProductionStorageContract : IProductionStorageContract
{
    private readonly GoToWorkDbContext _dbContext;
    private readonly Mapper _mapper;

    public ProductionStorageContract(GoToWorkDbContext dbContext)
    {
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Production, ProductionDataModel>()
                .ForMember(dest => dest.Details, opt => opt.MapFrom(x => x.DetailProductions));
            cfg.CreateMap<ProductionDataModel, Production>()
                .ForMember(dest => dest.DetailProductions, opt => opt.MapFrom(x => x.Details));
            cfg.CreateMap<Workshop, WorkshopDataModel>();
            cfg.CreateMap<WorkshopDataModel, Workshop>();
            cfg.CreateMap<Detail, DetailDataModel>();
            cfg.CreateMap<DetailDataModel, Detail>();
            cfg.CreateMap<DetailProduction, DetailProductionDataModel>();
            cfg.CreateMap<DetailProductionDataModel, DetailProduction>();
        });
        _mapper = new Mapper(config);
    }

    public List<ProductionDataModel> GetList()
    {
        try
        {
            return
            [
                .._dbContext.Productions
                    .Include(p => p.Workshops)
                    .Include(p => p.DetailProductions)!
                    .ThenInclude(d => d.Detail)
                    .Select(p => _mapper.Map<ProductionDataModel>(p))
            ];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public ProductionDataModel? GetElementById(string id)
    {
        try
        {
            return _mapper.Map<ProductionDataModel>(GetProductionById(id));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public ProductionDataModel? GetElementByName(string name)
    {
        try
        {
            return _mapper.Map<ProductionDataModel>(_dbContext.Productions.FirstOrDefault(p => p.Name == name));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void AddElement(ProductionDataModel productionDataModel)
    {
        try
        {
            _dbContext.Productions.Add(_mapper.Map<Production>(productionDataModel));
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex) when (ex.TargetSite?.Name == "ThrowIdentityConflict")
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", productionDataModel.Id);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void UpdElement(ProductionDataModel productionDataModel)
    {
        try
        {
            var existingProduction = GetProductionById(productionDataModel.Id)
                                     ?? throw new ElementNotFoundException(productionDataModel.Id);
            _dbContext.Productions.Update(_mapper.Map(productionDataModel, existingProduction));
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
            var production = GetProductionById(id) ?? throw new ElementNotFoundException(id);
            _dbContext.Productions.Remove(production);
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

    private Production? GetProductionById(string id) => _dbContext.Productions
        .Include(p => p.Workshops)
        .Include(p => p.DetailProductions)!
        .ThenInclude(d => d.Detail)
        .FirstOrDefault(p => p.Id == id);
}