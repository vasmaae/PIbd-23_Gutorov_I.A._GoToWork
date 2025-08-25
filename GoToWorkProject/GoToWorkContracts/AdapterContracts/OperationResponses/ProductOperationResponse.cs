using GoToWorkContracts.Infrastructure;
using GoToWorkContracts.ViewModels;

namespace GoToWorkContracts.AdapterContracts.OperationResponses;

public class ProductOperationResponse : OperationResponse
{
    public static ProductOperationResponse OK(List<ProductViewModel> data) =>
        OK<ProductOperationResponse, List<ProductViewModel>>(data);

    public static ProductOperationResponse OK(ProductViewModel data) =>
        OK<ProductOperationResponse, ProductViewModel>(data);

    public static ProductOperationResponse NoContent() =>
        NoContent<ProductOperationResponse>();

    public static ProductOperationResponse NotFound(string message) =>
        NotFound<ProductOperationResponse>(message);

    public static ProductOperationResponse BadRequest(string message) =>
        BadRequest<ProductOperationResponse>(message);

    public static ProductOperationResponse InternalServerError(string message) =>
        InternalServerError<ProductOperationResponse>(message);

    public static ProductOperationResponse Unauthorized(string message) =>
        Unauthorized<ProductOperationResponse>(message);
}