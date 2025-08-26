using System.Text.Json;
using System.Text.RegularExpressions;
using GoToWorkContracts.BusinessLogicContracts;
using GoToWorkContracts.DataModels;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.Extensions;
using GoToWorkContracts.StoragesContracts;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using GoToWorkContracts.Enums;

namespace GoToWorkBusinessLogic.Implementations;

public class UserBusinessLogicContract(
    IUserStorageContract userStorageContract,
    ILogger logger) : IUserBusinessLogicContract
{
    public List<UserDataModel> GetAllUsers()
    {
        logger.LogInformation("Getting all users");
        return userStorageContract.GetList() ?? throw new NullListException();
    }

    public UserDataModel GetUserByData(string data)
    {
        logger.LogInformation("Getting user by data: {data}", data);
        if (data.IsEmpty()) throw new ArgumentNullException(nameof(data));
        if (data.IsGuid())
            return userStorageContract.GetElementById(data) ?? throw new ElementNotFoundException(data);
        if (RegexExtensions.EmailRegex().IsMatch(data))
            return userStorageContract.GetElementByEmail(data) ?? throw new ElementNotFoundException(data);
        return userStorageContract.GetElementByLogin(data) ?? throw new ElementNotFoundException(data);
    }

    public void InsertUser(UserDataModel user)
    {
        logger.LogInformation("Inserting new user: {json}", JsonSerializer.Serialize(user));
        ArgumentNullException.ThrowIfNull(user);
        user.Validate();
        userStorageContract.AddElement(user);
    }

    public void UpdateUser(UserDataModel user)
    {
        logger.LogInformation("Updating user: {json}", JsonSerializer.Serialize(user));
        ArgumentNullException.ThrowIfNull(user);
        user.Validate();
        userStorageContract.UpdElement(user);
    }

    public void DeleteUser(string id)
    {
        logger.LogInformation("Deleting user with id: {id}", id);
        if (id.IsEmpty()) throw new ArgumentNullException(nameof(id));
        if (!id.IsGuid()) throw new ValidationException("Id is not a unique identifier");
        userStorageContract.DelElement(id);
    }

    public string Register(UserDataModel model)
    {
        logger.LogInformation("Registering new user: {json}", JsonSerializer.Serialize(model));
        ArgumentNullException.ThrowIfNull(model);

        var user = new UserDataModel(Guid.NewGuid().ToString(), model.Login, model.Email, model.Password, model.Role);
        user.Validate();
        user.Password = HashPassword(model.Password);

        if (userStorageContract.GetElementByLogin(user.Login) != null ||
            userStorageContract.GetElementByEmail(user.Email) != null)
        {
            throw new ElementExistsException(nameof(user.Login), user.Login);
        }

        userStorageContract.AddElement(user);
        return user.Id;
    }

    public (string id, string login, UserRole role)? Login(string loginOrEmail, string password)
    {
        logger.LogInformation("User login attempt: {login}", loginOrEmail);
        var user = RegexExtensions.EmailRegex().IsMatch(loginOrEmail)
            ? userStorageContract.GetElementByEmail(loginOrEmail)
            : userStorageContract.GetElementByLogin(loginOrEmail);

        if (user == null)
        {
            logger.LogWarning("User not found: {login}", loginOrEmail);
            return null;
        }

        if (user.Password != HashPassword(password))
        {
            logger.LogWarning("Invalid password for user: {login}", loginOrEmail);
            return null;
        }

        return (user.Id, user.Login, user.Role);
    }

    private static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
}