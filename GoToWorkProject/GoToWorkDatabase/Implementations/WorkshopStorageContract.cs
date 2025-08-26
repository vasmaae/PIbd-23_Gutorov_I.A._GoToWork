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
            cfg.CreateMap<Workshop, WorkshopDataModel>()
                .ForMember(dest => dest.Employees, opt => opt.MapFrom(src => src.EmployeeWorkshops));
            cfg.CreateMap<WorkshopDataModel, Workshop>()
                .ForMember(dest => dest.EmployeeWorkshops, opt => opt.MapFrom(src => src.Employees));
            cfg.CreateMap<Production, ProductionDataModel>();
            cfg.CreateMap<ProductionDataModel, Production>();
            cfg.CreateMap<Employee, EmployeeDataModel>();
            cfg.CreateMap<EmployeeDataModel, Employee>();
            cfg.CreateMap<EmployeeWorkshop, EmployeeWorkshopDataModel>();
            cfg.CreateMap<EmployeeWorkshopDataModel, EmployeeWorkshop>();
        });
        _mapper = new Mapper(config);
    }

    public List<WorkshopDataModel> GetList(string? productionId = null)
    {
        try
        {
            var query = _dbContext.Workshops
                .Include(x => x.Production)
                .Include(x => x.EmployeeWorkshops)!
                .ThenInclude(x => x.Employee)
                .AsQueryable();
            if (productionId is not null)
                query = query.Where(w => w.ProductionId == productionId);
            return [.. query.Select(w => _mapper.Map<WorkshopDataModel>(w))];
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
            return _mapper.Map<WorkshopDataModel>(GetWorkshopById(id));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public WorkshopDataModel? GetElementByAddress(string address)
    {
        try
        {
            var workshop = _dbContext.Workshops
                .Include(x => x.Production)
                .Include(x => x.EmployeeWorkshops)!
                .ThenInclude(x => x.Employee)
                .FirstOrDefault(w => w.Address == address);
            return _mapper.Map<WorkshopDataModel>(workshop);
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
            _dbContext.Workshops.Add(_mapper.Map<Workshop>(workshopDataModel));
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
            var existingWorkshop = GetWorkshopById(workshopDataModel.Id) ??
                                   throw new ElementNotFoundException(workshopDataModel.Id);
            _dbContext.Workshops.Update(_mapper.Map(workshopDataModel, existingWorkshop));
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
        .Include(x => x.Production)
        .Include(x => x.EmployeeWorkshops)!
        .ThenInclude(x => x.Employee)
        .FirstOrDefault(w => w.Id == id);
}