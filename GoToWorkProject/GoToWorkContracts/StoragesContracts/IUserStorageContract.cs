using GoToWorkContracts.DataModels;

namespace GoToWorkContracts.StoragesContracts;

public interface IUserStorageContract
{
    List<UserDataModel> GetList(bool? onlyActive = true);
    UserDataModel? GetElementById(string id);
    UserDataModel? GetElementByLogin(string login);
    UserDataModel? GetElementByEmail(string email);
    void AddElement(UserDataModel userDataModel);
    void UpdElement(UserDataModel userDataModel);
    void DelElement(string id);
    void ResElement(string id);
}