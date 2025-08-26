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
            cfg.CreateMap<Machine, MachineDataModel>()
                .ForMember(dest => dest.Employees, opt => opt.MapFrom(src => src.EmployeeMachines))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.MachineType));
            cfg.CreateMap<MachineDataModel, Machine>()
                .ForMember(dest => dest.EmployeeMachines, opt => opt.MapFrom(src => src.Employees))
                .ForMember(dest => dest.MachineType, opt => opt.MapFrom(src => src.Type));
            cfg.CreateMap<Employee, EmployeeDataModel>();
            cfg.CreateMap<EmployeeDataModel, Employee>();
            cfg.CreateMap<EmployeeMachine, EmployeeMachineDataModel>();
            cfg.CreateMap<EmployeeMachineDataModel, EmployeeMachine>();
            cfg.CreateMap<Product, ProductDataModel>();
            cfg.CreateMap<ProductDataModel, Product>();
        });
        _mapper = new Mapper(config);
    }

    public List<MachineDataModel> GetList()
    {
        try
        {
            return
            [
                .. _dbContext.Machines
                    .Include(x => x.Products)
                    .Include(x => x.EmployeeMachines)!
                    .ThenInclude(x => x.Employee)
                    .Select(m => _mapper.Map<MachineDataModel>(m))
            ];
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
            return _mapper.Map<MachineDataModel>(GetMachineById(id));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public MachineDataModel? GetElementByModel(string model)
    {
        try
        {
            return _mapper.Map<MachineDataModel>(_dbContext.Machines
                .Include(x => x.EmployeeMachines)!
                .ThenInclude(x => x.Employee)
                .Include(x => x.Products)
                .FirstOrDefault(m => m.Model == model));
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
            _dbContext.Machines.Add(_mapper.Map<Machine>(machineDataModel));
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
            _dbContext.Machines.Update(_mapper.Map(machineDataModel, existingMachine));
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

    private Machine? GetMachineById(string id)
    {
        return _dbContext.Machines
            .Include(x => x.EmployeeMachines)!
            .ThenInclude(x => x.Employee)
            .Include(x => x.Products)
            .FirstOrDefault(m => m.Id == id);
    }
}