using GoToWorkContracts.DataModels;

namespace GoToWorkContracts.BusinessLogicContracts;

public interface IUserBusinessLogicContract
{
    List<UserDataModel> GetAllUsers();
    UserDataModel GetUserByData(string data);
    void InsertUser(UserDataModel user);
    void UpdateUser(UserDataModel user);
    void DeleteUser(string id);
}