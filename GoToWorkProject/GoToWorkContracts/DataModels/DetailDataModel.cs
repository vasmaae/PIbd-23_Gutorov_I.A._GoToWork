using GoToWorkContracts.Enums;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.Extensions;
using GoToWorkContracts.Infrastructure;

namespace GoToWorkContracts.DataModels;

public class DetailDataModel : IValidation
{
    public string Id { get; set; }
    public string Name { get; set; }
    public MaterialType Material { get; set; }
    public DateTime CreationDate { get; set; }

    public DetailDataModel(string id, string name, MaterialType material, DateTime creationDate)
    {
        Id = id;
        Name = name;
        Material = material;
        CreationDate = creationDate.ToUniversalTime();
    }

    public DetailDataModel()
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
        if (Material == MaterialType.None)
            throw new ValidationException("Field Material is empty");
        if (CreationDate > DateTime.UtcNow)
            throw new ValidationException("The value in the field Id is not a valid Date");
    }
}