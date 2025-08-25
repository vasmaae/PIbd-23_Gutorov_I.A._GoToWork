using GoToWorkContracts.DataModels;

namespace GoToWorkContracts.BusinessLogicContracts;

public interface IMachineBusinessLogicContract
{
    List<MachineDataModel> GetAllMachines();
    List<MachineDataModel> GetMachinesByEmployee(string employeeId);
    MachineDataModel GetMachineByData(string data);
    void InsertMachine(MachineDataModel machine);
    void UpdateMachine(MachineDataModel machine);
    void DeleteMachine(string id);
}