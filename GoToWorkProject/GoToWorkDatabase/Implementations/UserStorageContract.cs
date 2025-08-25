using AutoMapper;
using GoToWorkContracts.DataModels;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.StoragesContracts;
using GoToWorkDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace GoToWorkDatabase.Implementations;

internal class UserStorageContract : IUserStorageContract
{
    private readonly GoToWorkDbContext _dbContext;
    private readonly Mapper _mapper;

    public UserStorageContract(GoToWorkDbContext dbContext)
    {
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<User, UserDataModel>();
            cfg.CreateMap<UserDataModel, User>();
        });
        _mapper = new Mapper(config);
    }

    public List<UserDataModel> GetList(bool? onlyActive = true)
    {
        try
        {
            var query = _dbContext.Users.AsQueryable();
            if (onlyActive == true) query = query.Where(u => !u.IsDeleted);
            return [.. query.Select(u => _mapper.Map<UserDataModel>(u))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public UserDataModel? GetElementById(string id)
    {
        try
        {
            var user = GetUserById(id);
            return user != null ? _mapper.Map<UserDataModel>(user) : null;
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public UserDataModel? GetElementByLogin(string login)
    {
        try
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Login == login);
            return user != null ? _mapper.Map<UserDataModel>(user) : null;
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public UserDataModel? GetElementByEmail(string email)
    {
        try
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            return user != null ? _mapper.Map<UserDataModel>(user) : null;
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void AddElement(UserDataModel userDataModel)
    {
        try
        {
            var user = _mapper.Map<User>(userDataModel);
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex) when (ex.TargetSite?.Name == "ThrowIdentityConflict")
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", userDataModel.Id);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void UpdElement(UserDataModel userDataModel)
    {
        try
        {
            var existingUser = GetUserById(userDataModel.Id)
                               ?? throw new ElementNotFoundException(userDataModel.Id);

            _mapper.Map(userDataModel, existingUser);
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
            var user = GetUserById(id) ?? throw new ElementNotFoundException(id);
            if (user.IsDeleted) throw new ElementDeletedException(id);
            user.IsDeleted = true;
            _dbContext.SaveChanges();
        }
        catch (Exception ex) when (ex is ElementNotFoundException or ElementDeletedException)
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
            var user = GetUserById(id) ?? throw new ElementNotFoundException(id);
            user.IsDeleted = false;
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

    private User? GetUserById(string id) => _dbContext.Users
        .Include(u => u.Details)
        .Include(u => u.Employees)
        .FirstOrDefault(u => u.Id == id);
}