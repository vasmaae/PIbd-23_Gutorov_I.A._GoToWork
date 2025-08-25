using GoToWorkContracts.DataModels;

namespace GoToWorkContracts.StoragesContracts;

public interface IEmployeeStorageContract
{
    List<EmployeeDataModel> GetList();
    EmployeeDataModel? GetElementById(string id);
    void AddElement(EmployeeDataModel employeeDataModel);
    void UpdElement(EmployeeDataModel employeeDataModel);
    void DelElement(string id);
}