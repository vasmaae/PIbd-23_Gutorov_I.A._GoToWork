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
            cfg.CreateMap<Production, ProductionDataModel>();
            cfg.CreateMap<ProductionDataModel, Production>();
            cfg.CreateMap<DetailProduction, DetailProductionDataModel>();
            cfg.CreateMap<DetailProductionDataModel, DetailProduction>();
        });
        _mapper = new Mapper(config);
    }

    public List<ProductionDataModel> GetList(string? workshopId = null, string? detailId = null)
    {
        try
        {
            var query = _dbContext.Productions
                .Include(p => p.Workshop)
                .Include(p => p.Details)!
                .ThenInclude(d => d.Detail)
                .AsQueryable();

            if (workshopId is not null)
                query = query.Where(p => p.WorkshopId == workshopId);

            if (detailId is not null)
                query = query.Where(p => p.Details!.Any(d => d.DetailId == detailId));

            return query
                .Select(p => _mapper.Map<ProductionDataModel>(p))
                .ToList();
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
            var production = GetProductionById(id);
            return production != null ? _mapper.Map<ProductionDataModel>(production) : null;
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
            var production = _mapper.Map<Production>(productionDataModel);
            _dbContext.Productions.Add(production);
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

            _mapper.Map(productionDataModel, existingProduction);
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

    public List<ProductionDataModel> GetElementsByWorkshopId(string workshopId)
    {
        try
        {
            var productions = _dbContext.Productions
                .Include(p => p.Workshop)
                .Include(p => p.Details)!
                .ThenInclude(d => d.Detail)
                .Where(p => p.WorkshopId == workshopId)
                .ToList();

            return productions
                .Select(p => _mapper.Map<ProductionDataModel>(p))
                .ToList();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public List<ProductionDataModel> GetElementsByAddress(string address)
    {
        try
        {
            var productions = _dbContext.Productions
                .Include(p => p.Workshop)
                .Include(p => p.Details)!
                .ThenInclude(d => d.Detail)
                .Where(p => p.Workshop!.Address == address)
                .ToList();

            return productions
                .Select(p => _mapper.Map<ProductionDataModel>(p))
                .ToList();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    private Production? GetProductionById(string id) => _dbContext.Productions
        .Include(p => p.Workshop)
        .Include(p => p.Details)!
        .ThenInclude(d => d.Detail)
        .FirstOrDefault(p => p.Id == id);
}