using GoToWorkContracts.DataModels;

namespace GoToWorkContracts.BusinessLogicContracts;

public interface IEmployeeBusinessLogicContract
{
    List<EmployeeDataModel> GetAllEmployees(bool onlyActive = true);
    EmployeeDataModel GetEmployeeByData(string data);
    void InsertEmployee(EmployeeDataModel employee);
    void UpdateEmployee(EmployeeDataModel employee);
    void DeleteEmployee(string id);
    void RestoreEmployee(string id);
}