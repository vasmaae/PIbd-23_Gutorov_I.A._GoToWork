using GoToWorkContracts.DataModels;

namespace GoToWorkContracts.StoragesContracts;

public interface IEmployeeStorageContract
{
    List<EmployeeDataModel> GetList(string? userId = null, bool onlyActive = true);
    EmployeeDataModel? GetElementById(string id);
    List<EmployeeDataModel> GetElementsByFullName(string fullName);
    void AddElement(EmployeeDataModel employeeDataModel);
    void UpdElement(EmployeeDataModel employeeDataModel);
    void DelElement(string id);
    void ResElement(string id);
}