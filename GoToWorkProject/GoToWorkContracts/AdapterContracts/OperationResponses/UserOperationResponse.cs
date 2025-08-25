using GoToWorkContracts.Infrastructure;
using GoToWorkContracts.ViewModels;

namespace GoToWorkContracts.AdapterContracts.OperationResponses;

public class UserOperationResponse : OperationResponse
{
    public static UserOperationResponse OK(List<UserViewModel> data) =>
        OK<UserOperationResponse, List<UserViewModel>>(data);

    public static UserOperationResponse OK(UserViewModel data) =>
        OK<UserOperationResponse, UserViewModel>(data);

    public static UserOperationResponse NoContent() =>
        NoContent<UserOperationResponse>();

    public static UserOperationResponse NotFound(string message) =>
        NotFound<UserOperationResponse>(message);

    public static UserOperationResponse BadRequest(string message) =>
        BadRequest<UserOperationResponse>(message);

    public static UserOperationResponse InternalServerError(string message) =>
        InternalServerError<UserOperationResponse>(message);

    public static UserOperationResponse Unauthorized(string message) =>
        Unauthorized<UserOperationResponse>(message);
}