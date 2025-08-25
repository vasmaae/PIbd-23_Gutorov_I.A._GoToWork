using GoToWorkContracts.Infrastructure;
using GoToWorkContracts.ViewModels;

namespace GoToWorkContracts.AdapterContracts.OperationResponses;

public class EmployeeOperationResponse : OperationResponse
{
    public static EmployeeOperationResponse OK(List<EmployeeViewModel> data) =>
        OK<EmployeeOperationResponse, List<EmployeeViewModel>>(data);

    public static EmployeeOperationResponse OK(EmployeeViewModel data) =>
        OK<EmployeeOperationResponse, EmployeeViewModel>(data);

    public static EmployeeOperationResponse NoContent() => 
        NoContent<EmployeeOperationResponse>();

    public static EmployeeOperationResponse NotFound(string message) => 
        NotFound<EmployeeOperationResponse>(message);

    public static EmployeeOperationResponse BadRequest(string message) =>
        BadRequest<EmployeeOperationResponse>(message);

    public static EmployeeOperationResponse InternalServerError(string message) =>
        InternalServerError<EmployeeOperationResponse>(message);

    public static EmployeeOperationResponse Unauthorized(string message) =>
        Unauthorized<EmployeeOperationResponse>(message);
}