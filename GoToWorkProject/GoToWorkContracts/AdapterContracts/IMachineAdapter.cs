using GoToWorkContracts.AdapterContracts.OperationResponses;
using GoToWorkContracts.BindingModels;

namespace GoToWorkContracts.AdapterContracts;

public interface IMachineAdapter
{
    MachineOperationResponse GetList();
    MachineOperationResponse GetElement(string data);
    MachineOperationResponse CreateMachine(MachineBindingModel machineModel);
    MachineOperationResponse UpdateMachine(MachineBindingModel machineModel);
    MachineOperationResponse DeleteMachine(string id);
}