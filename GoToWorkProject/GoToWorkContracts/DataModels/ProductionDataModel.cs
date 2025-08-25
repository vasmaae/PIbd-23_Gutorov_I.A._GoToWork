using GoToWorkContracts.Exceptions;
using GoToWorkContracts.Extensions;
using GoToWorkContracts.Infrastructure;

namespace GoToWorkContracts.DataModels;

public class ProductionDataModel(
    string id,
    string name,
    List<DetailProductionDataModel> details) : IValidation
{
    public string Id { get; } = id;
    public string Name { get; } = name;
    public List<DetailProductionDataModel>? Details { get; } = details;
    
    public void Validate()
    {
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");
        if (!Id.IsGuid())
            throw new ValidationException("The value in the field Id is not a Guid");
        if (Name.IsEmpty())
            throw new ValidationException("Field Name is empty");
        if ((Details?.Count ?? 0) == 0)
            throw new ValidationException("Production must include details");
    }
}