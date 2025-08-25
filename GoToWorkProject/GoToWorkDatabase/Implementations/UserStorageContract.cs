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

    public List<UserDataModel> GetList()
    {
        try
        {
            return [.. _dbContext.Users.Select(u => _mapper.Map<UserDataModel>(u))];
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
            return _mapper.Map<UserDataModel>(GetUserById(id));
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
            return _mapper.Map<UserDataModel>(_dbContext.Users.FirstOrDefault(u => u.Login == login));
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
            return _mapper.Map<UserDataModel>(_dbContext.Users.FirstOrDefault(u => u.Email == email));
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
            _dbContext.Users.Add(_mapper.Map<User>(userDataModel));
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
            _dbContext.Users.Remove(user);
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

    private User? GetUserById(string id) => _dbContext.Users.FirstOrDefault(u => u.Id == id);
}