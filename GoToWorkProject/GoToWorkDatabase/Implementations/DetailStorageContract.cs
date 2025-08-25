using AutoMapper;
using GoToWorkContracts.DataModels;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.StoragesContracts;
using GoToWorkDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace GoToWorkDatabase.Implementations;

internal class DetailStorageContract : IDetailStorageContract
{
    private readonly GoToWorkDbContext _dbContext;
    private readonly Mapper _mapper;

    public DetailStorageContract(GoToWorkDbContext dbContext)
    {
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Detail, DetailDataModel>();
            cfg.CreateMap<DetailDataModel, Detail>();
        });
        _mapper = new Mapper(config);
    }

    public List<DetailDataModel> GetList(DateTime? startDate = null, DateTime? endDate = null)
    {
        try
        {
            var query = _dbContext.Details.AsQueryable();
            if (startDate is not null)
                query = query.Where(x => x.CreationDate >= startDate);
            if (endDate is not null)
                query = query.Where(x => x.CreationDate <= endDate);
            return [.. query.Select(d => _mapper.Map<DetailDataModel>(d))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public DetailDataModel? GetElementById(string id)
    {
        try
        {
            return _mapper.Map<DetailDataModel>(GetDetailById(id));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public DetailDataModel? GetElementByName(string name)
    {
        try
        {
            return _mapper.Map<DetailDataModel>(_dbContext.Details.FirstOrDefault(d => d.Name == name));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void AddElement(DetailDataModel detailDataModel)
    {
        try
        {
            _dbContext.Details.Add(_mapper.Map<Detail>(detailDataModel));
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex) when (ex.TargetSite?.Name == "ThrowIdentityConflict")
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", detailDataModel.Id);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void UpdElement(DetailDataModel detailDataModel)
    {
        try
        {
            var element = GetDetailById(detailDataModel.Id)
                          ?? throw new ElementNotFoundException(detailDataModel.Id);
            _dbContext.Details.Update(_mapper.Map(detailDataModel, element));
            _dbContext.SaveChanges();
        }
        catch (ElementNotFoundException ex)
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
            var element = GetDetailById(id) ?? throw new ElementNotFoundException(id);
            _dbContext.Details.Remove(element);
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

    private Detail? GetDetailById(string id) => _dbContext.Details.FirstOrDefault(d => d.Id == id);
}