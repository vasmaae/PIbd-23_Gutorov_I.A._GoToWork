using System.Text.Json;
using GoToWorkContracts.BusinessLogicContracts;
using GoToWorkContracts.DataModels;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.Extensions;
using GoToWorkContracts.StoragesContracts;
using Microsoft.Extensions.Logging;

namespace GoToWorkBusinessLogic.Implementations;

internal class EmployeeBusinessLogicContract(
    IEmployeeStorageContract employeeStorageContract,
    ILogger logger) : IEmployeeBusinessLogicContract
{
    public List<EmployeeDataModel> GetAllEmployees()
    {
        logger.LogInformation("Getting all employees");
        return employeeStorageContract.GetList()
               ?? [];
    }

    public EmployeeDataModel GetEmployeeByData(string data)
    {
        logger.LogInformation("Getting employee by id: {data}", data);
        if (data.IsEmpty()) throw new ArgumentNullException(nameof(data));
        if (data.IsGuid())
            return employeeStorageContract.GetElementById(data) ?? throw new ElementNotFoundException(data);
        throw new ElementNotFoundException(data);
    }

    public void InsertEmployee(EmployeeDataModel employee)
    {
        logger.LogInformation("Inserting new employee: {json}", JsonSerializer.Serialize(employee));
        ArgumentNullException.ThrowIfNull(employee);
        employee.Validate();
        employeeStorageContract.AddElement(employee);
    }

    public void UpdateEmployee(EmployeeDataModel employee)
    {
        logger.LogInformation("Updating employee: {json}", JsonSerializer.Serialize(employee));
        ArgumentNullException.ThrowIfNull(employee);
        employee.Validate();
        employeeStorageContract.UpdElement(employee);
    }

    public void DeleteEmployee(string id)
    {
        logger.LogInformation("Deleting employee with id: {id}", id);
        if (id.IsEmpty()) throw new ArgumentNullException(nameof(id));
        if (!id.IsGuid()) throw new ValidationException("Id is not a unique identifier");
        employeeStorageContract.DelElement(id);
    }
}