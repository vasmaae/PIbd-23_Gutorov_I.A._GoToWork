using AutoMapper;
using GoToWorkContracts.DataModels;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.StoragesContracts;
using GoToWorkContracts.ViewModels;
using GoToWorkDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace GoToWorkDatabase.Implementations;

internal class EmployeeStorageContract : IEmployeeStorageContract
{
    private readonly GoToWorkDbContext _dbContext;
    private readonly Mapper _mapper;

    public EmployeeStorageContract(GoToWorkDbContext dbContext)
    {
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Employee, EmployeeDataModel>();
            cfg.CreateMap<EmployeeDataModel, Employee>();
            cfg.CreateMap<Machine, MachineDataModel>();
            cfg.CreateMap<MachineDataModel, Machine>();
            cfg.CreateMap<Workshop, WorkshopDataModel>();
            cfg.CreateMap<WorkshopDataModel, Workshop>();
            cfg.CreateMap<EmployeeMachine, EmployeeMachineDataModel>();
            cfg.CreateMap<EmployeeMachineDataModel, EmployeeMachine>();
            cfg.CreateMap<EmployeeWorkshop, EmployeeWorkshopDataModel>();
            cfg.CreateMap<EmployeeWorkshopDataModel, EmployeeWorkshop>();
        });
        _mapper = new Mapper(config);
    }

    public List<EmployeeDataModel> GetList()
    {
        try
        {
            return
            [
                .. _dbContext.Employees
                    .Include(x => x.EmployeeMachines)!
                    .ThenInclude(x => x.Machine)
                    .Include(x => x.EmployeeWorkshops)!
                    .ThenInclude(x => x.Workshop)
                    .Select(x => _mapper.Map<EmployeeDataModel>(x))
            ];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public EmployeeDataModel? GetElementById(string id)
    {
        try
        {
            return _mapper.Map<EmployeeDataModel>(GetEmployeeById(id));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public List<EmployeeDataModel> GetElementsByFullName(string fullName)
    {
        try
        {
            return
            [
                .. _dbContext.Employees
                    .Include(x => x.EmployeeMachines)!
                    .ThenInclude(x => x.Machine)
                    .Include(x => x.EmployeeWorkshops)!
                    .ThenInclude(x => x.Workshop)
                    .Where(e => e.FullName == fullName)
                    .Select(x => _mapper.Map<EmployeeDataModel>(x))
            ];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void AddElement(EmployeeDataModel employeeDataModel)
    {
        try
        {
            _dbContext.Employees.Add(_mapper.Map<Employee>(employeeDataModel));
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex) when (ex.TargetSite?.Name == "ThrowIdentityConflict")
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", employeeDataModel.Id);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void UpdElement(EmployeeDataModel employeeDataModel)
    {
        try
        {
            var existing = GetEmployeeById(employeeDataModel.Id) ??
                           throw new ElementNotFoundException(employeeDataModel.Id);
            _dbContext.Employees.Update(_mapper.Map(employeeDataModel, existing));
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
            var element = GetEmployeeById(id) ?? throw new ElementNotFoundException(id);
            _dbContext.Employees.Remove(element);
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

    private Employee? GetEmployeeById(string id) => _dbContext.Employees
        .Include(x => x.EmployeeMachines)!
        .ThenInclude(x => x.Machine)
        .Include(x => x.EmployeeWorkshops)!
        .ThenInclude(x => x.Workshop)
        .FirstOrDefault(e => e.Id == id);
}