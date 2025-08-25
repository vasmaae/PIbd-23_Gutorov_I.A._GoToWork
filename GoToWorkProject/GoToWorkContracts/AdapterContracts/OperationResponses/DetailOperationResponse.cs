using GoToWorkContracts.Infrastructure;
using GoToWorkContracts.ViewModels;

namespace GoToWorkContracts.AdapterContracts.OperationResponses;

public class DetailOperationResponse : OperationResponse
{
    public static DetailOperationResponse OK(List<DetailViewModel> data) =>
        OK<DetailOperationResponse, List<DetailViewModel>>(data);

    public static DetailOperationResponse OK(DetailViewModel data) =>
        OK<DetailOperationResponse, DetailViewModel>(data);

    public static DetailOperationResponse NoContent() =>
        NoContent<DetailOperationResponse>();

    public static DetailOperationResponse NotFound(string message) =>
        NotFound<DetailOperationResponse>(message);

    public static DetailOperationResponse BadRequest(string message) =>
        BadRequest<DetailOperationResponse>(message);

    public static DetailOperationResponse InternalServerError(string message) =>
        InternalServerError<DetailOperationResponse>(message);

    public static DetailOperationResponse Unauthorized(string message) =>
        Unauthorized<DetailOperationResponse>(message);
}