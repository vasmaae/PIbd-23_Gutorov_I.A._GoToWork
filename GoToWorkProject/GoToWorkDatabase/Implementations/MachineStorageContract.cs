using AutoMapper;
using GoToWorkContracts.DataModels;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.StoragesContracts;
using GoToWorkDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace GoToWorkDatabase.Implementations;

internal class MachineStorageContract : IMachineStorageContract
{
    private readonly GoToWorkDbContext _dbContext;
    private readonly Mapper _mapper;

    public MachineStorageContract(GoToWorkDbContext dbContext)
    {
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Machine, MachineDataModel>();
            cfg.CreateMap<MachineDataModel, Machine>();
            cfg.CreateMap<EmployeeMachine, EmployeeMachineDataModel>();
            cfg.CreateMap<EmployeeMachineDataModel, EmployeeMachine>();
        });
        _mapper = new Mapper(config);
    }

    public List<MachineDataModel> GetList(string? employeeId = null)
    {
        try
        {
            var query = _dbContext.Machines
                .Include(m => m.Employees)!
                .ThenInclude(em => em.Employee)
                .AsQueryable();

            if (employeeId is not null)
                query = query.Where(m => m.Employees!.Any(em => em.EmployeeId == employeeId));

            return query
                .Select(m => _mapper.Map<MachineDataModel>(m))
                .ToList();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public MachineDataModel? GetElementById(string id)
    {
        try
        {
            var machine = GetMachineById(id);
            return machine != null ? _mapper.Map<MachineDataModel>(machine) : null;
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public List<MachineDataModel> GetElementsByModel(string model)
    {
        try
        {
            var machines = _dbContext.Machines
                .Include(m => m.Employees)!
                .ThenInclude(em => em.Employee)
                .Where(m => m.Model == model)
                .ToList();

            return machines
                .Select(m => _mapper.Map<MachineDataModel>(m))
                .ToList();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void AddElement(MachineDataModel machineDataModel)
    {
        try
        {
            var machine = _mapper.Map<Machine>(machineDataModel);
            _dbContext.Machines.Add(machine);
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex) when (ex.TargetSite?.Name == "ThrowIdentityConflict")
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", machineDataModel.Id);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void UpdElement(MachineDataModel machineDataModel)
    {
        try
        {
            var existingMachine = GetMachineById(machineDataModel.Id)
                                  ?? throw new ElementNotFoundException(machineDataModel.Id);

            _mapper.Map(machineDataModel, existingMachine);
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
            var machine = GetMachineById(id) ?? throw new ElementNotFoundException(id);
            _dbContext.Machines.Remove(machine);
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

    private Machine? GetMachineById(string id) => _dbContext.Machines
        .Include(m => m.Employees)!
        .ThenInclude(em => em.Employee)
        .FirstOrDefault(m => m.Id == id);
}