using GoToWorkContracts.Enums;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.Extensions;
using GoToWorkContracts.Infrastructure;

namespace GoToWorkContracts.DataModels;

public class DetailDataModel(
    string id,
    string name,
    MaterialType material)
    : IValidation
{
    public string Id { get; } = id;
    public string Name { get; } = name;
    public MaterialType Material { get; } = material;

    public void Validate()
    {
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");
        if (!Id.IsGuid())
            throw new ValidationException("The value in the field Id is not a Guid");
        if (Name.IsEmpty())
            throw new ValidationException("Field Name is empty");
        if (Material == MaterialType.None)
            throw new ValidationException("Field Material is empty");
    }
}