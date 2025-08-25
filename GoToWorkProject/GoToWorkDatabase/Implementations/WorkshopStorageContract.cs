using AutoMapper;
using GoToWorkContracts.DataModels;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.StoragesContracts;
using GoToWorkDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace GoToWorkDatabase.Implementations;

internal class WorkshopStorageContract : IWorkshopStorageContract
{
    private readonly GoToWorkDbContext _dbContext;
    private readonly Mapper _mapper;

    public WorkshopStorageContract(GoToWorkDbContext dbContext)
    {
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Workshop, WorkshopDataModel>();
            cfg.CreateMap<WorkshopDataModel, Workshop>();
            cfg.CreateMap<EmployeeWorkshop, EmployeeWorkshopDataModel>();
            cfg.CreateMap<EmployeeWorkshopDataModel, EmployeeWorkshop>();
        });
        _mapper = new Mapper(config);
    }

    public List<WorkshopDataModel> GetList(string? employeeId = null)
    {
        try
        {
            var query = _dbContext.Workshops
                .Include(w => w.EmployeeWorkshops)!
                .ThenInclude(ew => ew.Employee)
                .AsQueryable();

            if (!string.IsNullOrEmpty(employeeId))
                query = query.Where(w => w.EmployeeWorkshops!.Any(ew => ew.EmployeeId == employeeId));

            return query
                .Select(w => _mapper.Map<WorkshopDataModel>(w))
                .ToList();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public WorkshopDataModel? GetElementById(string id)
    {
        try
        {
            var workshop = GetWorkshopById(id);
            return workshop != null ? _mapper.Map<WorkshopDataModel>(workshop) : null;
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public List<WorkshopDataModel> GetElementsByAddress(string address)
    {
        try
        {
            var workshops = _dbContext.Workshops
                .Include(w => w.EmployeeWorkshops)!
                .ThenInclude(ew => ew.Employee)
                .Where(w => w.Address == address)
                .ToList();

            return workshops
                .Select(w => _mapper.Map<WorkshopDataModel>(w))
                .ToList();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void AddElement(WorkshopDataModel workshopDataModel)
    {
        try
        {
            var workshop = _mapper.Map<Workshop>(workshopDataModel);
            _dbContext.Workshops.Add(workshop);
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex) when (ex.TargetSite?.Name == "ThrowIdentityConflict")
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", workshopDataModel.Id);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void UpdElement(WorkshopDataModel workshopDataModel)
    {
        try
        {
            var existingWorkshop = GetWorkshopById(workshopDataModel.Id)
                                   ?? throw new ElementNotFoundException(workshopDataModel.Id);

            _mapper.Map(workshopDataModel, existingWorkshop);
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
            var workshop = GetWorkshopById(id) ?? throw new ElementNotFoundException(id);
            _dbContext.Workshops.Remove(workshop);
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

    private Workshop? GetWorkshopById(string id) => _dbContext.Workshops
        .Include(w => w.EmployeeWorkshops)!
        .ThenInclude(ew => ew.Employee)
        .FirstOrDefault(w => w.Id == id);
}