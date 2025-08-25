using GoToWorkContracts.Infrastructure;
using GoToWorkContracts.ViewModels;

namespace GoToWorkContracts.AdapterContracts.OperationResponses;

public class MachineOperationResponse : OperationResponse
{
    public static MachineOperationResponse OK(List<MachineViewModel> data) =>
        OK<MachineOperationResponse, List<MachineViewModel>>(data);

    public static MachineOperationResponse OK(MachineViewModel data) =>
        OK<MachineOperationResponse, MachineViewModel>(data);

    public static MachineOperationResponse NoContent() =>
        NoContent<MachineOperationResponse>();

    public static MachineOperationResponse NotFound(string message) =>
        NotFound<MachineOperationResponse>(message);

    public static MachineOperationResponse BadRequest(string message) =>
        BadRequest<MachineOperationResponse>(message);

    public static MachineOperationResponse InternalServerError(string message) =>
        InternalServerError<MachineOperationResponse>(message);

    public static MachineOperationResponse Unauthorized(string message) =>
        Unauthorized<MachineOperationResponse>(message);
}