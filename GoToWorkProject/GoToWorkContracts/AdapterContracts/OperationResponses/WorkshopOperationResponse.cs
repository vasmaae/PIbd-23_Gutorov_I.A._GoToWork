using GoToWorkContracts.Infrastructure;
using GoToWorkContracts.ViewModels;

namespace GoToWorkContracts.AdapterContracts.OperationResponses;

public class WorkshopOperationResponse : OperationResponse
{
    public static WorkshopOperationResponse OK(List<WorkshopViewModel> data) =>
        OK<WorkshopOperationResponse, List<WorkshopViewModel>>(data);

    public static WorkshopOperationResponse OK(WorkshopViewModel data) =>
        OK<WorkshopOperationResponse, WorkshopViewModel>(data);

    public static WorkshopOperationResponse NoContent() =>
        NoContent<WorkshopOperationResponse>();

    public static WorkshopOperationResponse NotFound(string message) =>
        NotFound<WorkshopOperationResponse>(message);

    public static WorkshopOperationResponse BadRequest(string message) =>
        BadRequest<WorkshopOperationResponse>(message);

    public static WorkshopOperationResponse InternalServerError(string message) =>
        InternalServerError<WorkshopOperationResponse>(message);

    public static WorkshopOperationResponse Unauthorized(string message) =>
        Unauthorized<WorkshopOperationResponse>(message);
}