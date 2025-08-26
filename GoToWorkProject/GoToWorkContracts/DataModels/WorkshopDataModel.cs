using GoToWorkContracts.Exceptions;
using GoToWorkContracts.Extensions;
using GoToWorkContracts.Infrastructure;

namespace GoToWorkContracts.DataModels;

public class WorkshopDataModel : IValidation
{
    public string Id { get; set; }
    public string? ProductionId { get; set; }
    public string Address { get; set; }
    public List<EmployeeWorkshopDataModel>? Employees { get; set; }
    private ProductionDataModel? _production;
    public string ProductionName => _production?.Name ?? string.Empty;

    public WorkshopDataModel() { }

    public WorkshopDataModel(string id, string? productionId, string address, List<EmployeeWorkshopDataModel> employees)
    {
        Id = id;
        ProductionId = productionId;
        Address = address;
        Employees = employees;
    }

    public WorkshopDataModel(string id, string? productionId, string address, List<EmployeeWorkshopDataModel> employees,
        ProductionDataModel? production) : this(id, productionId, address, employees)
    {
        _production = production;
    }

    public void Validate()
    {
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");
        if (!Id.IsGuid())
            throw new ValidationException("The value in the field Id is not a Guid");
        if (Address.IsEmpty())
            throw new ValidationException("Field Address is empty");
        if ((Employees?.Count ?? 0) == 0)
            throw new ValidationException("Workshop must include employees");
        if (ProductionId is not null)
        {
            if (ProductionId.IsEmpty())
                throw new ValidationException("Field ProductionId is empty");
            if (!ProductionId.IsGuid())
                throw new ValidationException("The value in the field ProductionId is not a Guid");
        }
    }
}