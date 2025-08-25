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
        });
        _mapper = new Mapper(config);
    }

    public List<MachineDataModel> GetList()
    {
        try
        {
            return [.. _dbContext.Machines.Select(m => _mapper.Map<MachineDataModel>(m))];
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
            return _mapper.Map<MachineDataModel>(_dbContext.Machines.FirstOrDefault(m => m.Model == model));
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
        return _dbContext.Machines.FirstOrDefault(m => m.Id == id);
    }
}