using GoToWorkContracts.DataModels;
using GoToWorkContracts.Enums;

namespace GoToWorkContracts.BusinessLogicContracts;

public interface IUserBusinessLogicContract
{
    List<UserDataModel> GetAllUsers();
    UserDataModel GetUserByData(string data);
    void InsertUser(UserDataModel user);
    void UpdateUser(UserDataModel user);
    void DeleteUser(string id);
    string Register(UserDataModel model);
    (string id, string login, UserRole role)? Login(string loginOrEmail, string password);
}