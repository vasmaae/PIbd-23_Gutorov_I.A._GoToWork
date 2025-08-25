using GoToWorkContracts.Exceptions;
using GoToWorkContracts.Extensions;
using GoToWorkContracts.Infrastructure;

namespace GoToWorkContracts.DataModels;

public class EmployeeMachineDataModel(
    string employeeId,
    string machineId)
    : IValidation
{
    public string EmployeeId { get; set; } = employeeId;
    public string MachineId { get; set; } = machineId;
    private readonly EmployeeDataModel? _employee;
    private readonly MachineDataModel? _machine;
    public string EmployeeName => _employee?.FullName ?? string.Empty;
    public string MachineName => _machine?.Model ?? string.Empty;

    public EmployeeMachineDataModel(string employeeId, string machineId, EmployeeDataModel employee,
        MachineDataModel machine) : this(employeeId, machineId)
    {
        _employee = employee;
        _machine = machine;
    }

    public void Validate()
    {
        if (EmployeeId.IsEmpty())
            throw new ValidationException("Field EmployeeId is empty");
        if (!EmployeeId.IsGuid())
            throw new ValidationException("The value in the field EmployeeId is not a Guid");
        if (MachineId.IsEmpty())
            throw new ValidationException("Field MachineId is empty");
        if (!MachineId.IsGuid())
            throw new ValidationException("The value in the field MachineId is not a Guid");
    }
}