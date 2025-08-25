using GoToWorkContracts.Enums;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.Extensions;
using GoToWorkContracts.Infrastructure;

namespace GoToWorkContracts.DataModels;

public class DetailProductionDataModel(
    string detailId,
    string productionId)
    : IValidation
{
    public string DetailId { get; } = detailId;
    public string ProductionId { get; } = productionId;
    private readonly DetailDataModel? _detail;
    private readonly ProductionDataModel? _production;
    public string DetailName => _detail?.Name ?? string.Empty;
    public string ProductionName => _production?.Name ?? string.Empty;

    public DetailProductionDataModel(string detailId, string productionId, DetailDataModel? detail,
        ProductionDataModel? production) : this(detailId, productionId)
    {
        _detail = detail;
        _production = production;
    }

    public void Validate()
    {
        if (ProductionId.IsEmpty())
            throw new ValidationException("Field ProductionId is empty");
        if (!ProductionId.IsGuid())
            throw new ValidationException("The value in the field ProductionId is not a Guid");
        if (DetailId.IsEmpty())
            throw new ValidationException("Field DetailId is empty");
        if (!DetailId.IsGuid())
            throw new ValidationException("The value in the field DetailId is not a Guid");
    }
}