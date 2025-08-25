using System.Text.Json;
using System.Text.RegularExpressions;
using GoToWorkContracts.BusinessLogicContracts;
using GoToWorkContracts.DataModels;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.Extensions;
using GoToWorkContracts.StoragesContracts;
using Microsoft.Extensions.Logging;

namespace GoToWorkBusinessLogic.Implementations;

internal class UserBusinessLogicContract(
    IUserStorageContract userStorageContract,
    ILogger logger) : IUserBusinessLogicContract
{
    public List<UserDataModel> GetAllUsers(bool onlyActive = true)
    {
        logger.LogInformation("Getting all users (onlyActive: {onlyActive})", onlyActive);
        return userStorageContract.GetList(onlyActive) 
               ?? throw new NullListException();
    }

    public UserDataModel GetUserByData(string data)
    {
        logger.LogInformation("Getting user by data: {data}", data);
        if (data.IsEmpty()) throw new ArgumentNullException(nameof(data));
        if (data.IsGuid())
            return userStorageContract.GetElementById(data)
                   ?? throw new ElementNotFoundException(data);
        if (RegexExtensions.EmailRegex().IsMatch(data))
            return userStorageContract.GetElementByEmail(data)
                   ?? throw new ElementNotFoundException(data);
        return userStorageContract.GetElementByLogin(data)
               ?? throw new ElementNotFoundException(data);
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

    public void RestoreUser(string id)
    {
        logger.LogInformation("Restoring user with id: {id}", id);
        if (id.IsEmpty()) throw new ArgumentNullException(nameof(id));
        if (!id.IsGuid()) throw new ValidationException("Id is not a unique identifier");
        userStorageContract.ResElement(id);
    }
}