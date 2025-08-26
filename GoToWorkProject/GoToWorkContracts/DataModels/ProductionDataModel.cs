using GoToWorkContracts.Exceptions;
using GoToWorkContracts.Extensions;
using GoToWorkContracts.Infrastructure;

namespace GoToWorkContracts.DataModels;

public class ProductionDataModel : IValidation
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<DetailProductionDataModel>? Details { get; set; }
    public List<WorkshopDataModel>? Workshops { get; set; }

    public ProductionDataModel(string id, string name, List<DetailProductionDataModel> details,
        List<WorkshopDataModel>? workshops)
    {
        Id = id;
        Name = name;
        Details = details;
        Workshops = workshops;
    }

    public ProductionDataModel()
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
    }
}