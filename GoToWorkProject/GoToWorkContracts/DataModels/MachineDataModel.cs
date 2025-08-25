using GoToWorkContracts.Enums;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.Extensions;
using GoToWorkContracts.Infrastructure;

namespace GoToWorkContracts.DataModels;

public class MachineDataModel(
    string id,
    string model,
    MachineType type,
    List<EmployeeMachineDataModel> employees) : IValidation
{
    public string Id { get; set; } = id;
    public string Model { get; set; } = model;
    public MachineType Type { get; set; } = type;
    public List<EmployeeMachineDataModel>? Employees { get; set; } = employees;

    public void Validate()
    {
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");
        if (!Id.IsGuid())
            throw new ValidationException("The value in the field Id is not a Guid");
        if (Model.IsEmpty())
            throw new ValidationException("Field Model is empty");
        if (Type == MachineType.None)
            throw new ValidationException("Field Machine is empty");
        if ((Employees?.Count ?? 0) == 0)
            throw new ValidationException("Machine must include employees");
    }
}