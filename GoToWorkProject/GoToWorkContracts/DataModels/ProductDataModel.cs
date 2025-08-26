using GoToWorkContracts.Exceptions;
using GoToWorkContracts.Extensions;
using GoToWorkContracts.Infrastructure;

namespace GoToWorkContracts.DataModels;

public class ProductDataModel : IValidation
{
    public string Id { get; set; }
    public string? MachineId { get; set; }
    public string Name { get; set; }
    public DateTime CreationDate { get; set; }
    public List<DetailProductDataModel>? Details { get; set; }
    public string MachineName => Machine?.Model ?? string.Empty;
    public MachineDataModel? Machine { get; set; }

    public ProductDataModel(string id, string? machineId, string name, DateTime creationDate,
        List<DetailProductDataModel> details)
    {
        Id = id;
        MachineId = machineId;
        Name = name;
        CreationDate = creationDate;
        Details = details;
    }

    public ProductDataModel(string id, string? machineId, string name, DateTime creationDate,
        List<DetailProductDataModel> details, MachineDataModel? machine) : this(id, machineId, name, creationDate,
        details)
    {
        Machine = machine;
    }

    public ProductDataModel()
    {
    }

    public void Validate()
    {
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");
        if (!Id.IsGuid())
            throw new ValidationException("The value in the field Id is not a Guid");
        if (Name.IsEmpty())
            throw new ValidationException("Field Name is empty");
        if (MachineId is not null)
        {
            if (MachineId.IsEmpty())
                throw new ValidationException("Field MachineId is empty");
            if (!MachineId.IsGuid())
                throw new ValidationException("The value in the field MachineId is not a Guid");
        }
    }
}