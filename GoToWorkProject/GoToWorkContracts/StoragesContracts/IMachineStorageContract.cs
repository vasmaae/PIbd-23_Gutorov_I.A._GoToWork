using GoToWorkContracts.DataModels;

namespace GoToWorkContracts.StoragesContracts;

public interface IMachineStorageContract
{
    List<MachineDataModel> GetList(string? employeeId = null);
    MachineDataModel? GetElementById(string id);
    List<MachineDataModel> GetElementsByModel(string model);
    void AddElement(MachineDataModel machineDataModel);
    void UpdElement(MachineDataModel machineDataModel);
    void DelElement(string id);
}