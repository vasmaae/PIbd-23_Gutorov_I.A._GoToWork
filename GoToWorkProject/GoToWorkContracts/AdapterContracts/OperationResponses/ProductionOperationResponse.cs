using GoToWorkContracts.Infrastructure;
using GoToWorkContracts.ViewModels;

namespace GoToWorkContracts.AdapterContracts.OperationResponses;

public class ProductionOperationResponse : OperationResponse
{
    public static ProductionOperationResponse OK(List<ProductionViewModel> data) =>
        OK<ProductionOperationResponse, List<ProductionViewModel>>(data);

    public static ProductionOperationResponse OK(ProductionViewModel data) =>
        OK<ProductionOperationResponse, ProductionViewModel>(data);

    public static ProductionOperationResponse NoContent() =>
        NoContent<ProductionOperationResponse>();

    public static ProductionOperationResponse NotFound(string message) =>
        NotFound<ProductionOperationResponse>(message);

    public static ProductionOperationResponse BadRequest(string message) =>
        BadRequest<ProductionOperationResponse>(message);

    public static ProductionOperationResponse InternalServerError(string message) =>
        InternalServerError<ProductionOperationResponse>(message);

    public static ProductionOperationResponse Unauthorized(string message) =>
        Unauthorized<ProductionOperationResponse>(message);
}