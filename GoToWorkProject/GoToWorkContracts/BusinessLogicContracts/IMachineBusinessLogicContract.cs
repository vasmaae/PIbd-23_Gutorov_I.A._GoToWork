using GoToWorkContracts.DataModels;

namespace GoToWorkContracts.BusinessLogicContracts;

public interface IMachineBusinessLogicContract
{
    List<MachineDataModel> GetAllMachines();
    MachineDataModel GetMachineByData(string data);
    void InsertMachine(MachineDataModel machine);
    void UpdateMachine(MachineDataModel machine);
    void DeleteMachine(string id);
}