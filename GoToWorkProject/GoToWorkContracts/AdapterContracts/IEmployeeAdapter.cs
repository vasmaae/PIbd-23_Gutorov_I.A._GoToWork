using GoToWorkContracts.AdapterContracts.OperationResponses;
using GoToWorkContracts.BindingModels;

namespace GoToWorkContracts.AdapterContracts;

public interface IEmployeeAdapter
{
    EmployeeOperationResponse GetList();
    EmployeeOperationResponse GetElement(string data);
    EmployeeOperationResponse CreateEmployee(EmployeeBindingModel employeeModel);
    EmployeeOperationResponse UpdateEmployee(EmployeeBindingModel employeeModel);
    EmployeeOperationResponse DeleteEmployee(string id);
}