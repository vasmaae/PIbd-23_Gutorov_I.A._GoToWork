using AutoMapper;
using GoToWorkContracts.DataModels;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.StoragesContracts;
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
        });
        _mapper = new Mapper(config);
    }

    public List<EmployeeDataModel> GetList(string? userId = null, bool onlyActive = true)
    {
        try
        {
            var query = _dbContext.Employees.AsQueryable();
            if (onlyActive) query = query.Where(x => !x.IsDeleted);
            if (userId is not null) query = query.Where(x => x.UserId == userId);
            return [.. query.Select(x => _mapper.Map<EmployeeDataModel>(x))];
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
            return _mapper.Map<List<EmployeeDataModel>>(_dbContext.Employees
                .Select(x => x.FullName == fullName));
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
            var entity = GetEmployeeById(id) ?? throw new ElementNotFoundException(id);
            entity.IsDeleted = true;
            entity.DateOfDelete = DateTime.UtcNow;
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

    public void ResElement(string id)
    {
        try
        {
            var entity = GetEmployeeById(id) ?? throw new ElementNotFoundException(id);
            entity.IsDeleted = false;
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
        .Include(x => x.User)
        .Include(x => x.Machines)
        .Include(x => x.Workshops)
        .FirstOrDefault(e => e.Id == id);
}