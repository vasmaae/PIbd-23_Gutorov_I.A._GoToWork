using GoToWorkContracts.Exceptions;
using GoToWorkContracts.Extensions;
using GoToWorkContracts.Infrastructure;

namespace GoToWorkContracts.DataModels;

public class ProductDataModel(
    string id,
    string? machineId,
    string name,
    DateTime creationDate,
    List<DetailProductDataModel> details) : IValidation
{
    public string Id { get; } = id;
    public string? MachineId { get; } = machineId;
    public string Name { get; } = name;
    public DateTime CreationDate { get; set; } = creationDate;
    public List<DetailProductDataModel>? Details { get; set; } = details;
    private readonly MachineDataModel? _machine;
    public string MachineName => _machine?.Model ?? string.Empty;

    public ProductDataModel(string id, string? machineId, string name, DateTime creationDate,
        List<DetailProductDataModel> details, MachineDataModel? machine) : this(id, machineId, name, creationDate,
        details)
    {
        _machine = machine;
    }

    public void Validate()
    {
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");
        if (!Id.IsGuid())
            throw new ValidationException("The value in the field Id is not a Guid");
        if (Name.IsEmpty())
            throw new ValidationException("Field Name is empty");
        if ((Details?.Count ?? 0) == 0)
            throw new ValidationException("Product must include details");
        if (MachineId is not null)
        {
            if (MachineId.IsEmpty())
                throw new ValidationException("Field MachineId is empty");
            if (!MachineId.IsGuid())
                throw new ValidationException("The value in the field MachineId is not a Guid");
        }
    }
}