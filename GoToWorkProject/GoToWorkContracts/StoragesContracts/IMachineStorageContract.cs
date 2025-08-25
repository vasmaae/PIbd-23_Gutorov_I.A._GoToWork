using GoToWorkContracts.DataModels;

namespace GoToWorkContracts.StoragesContracts;

public interface IMachineStorageContract
{
    List<MachineDataModel> GetList();
    MachineDataModel? GetElementById(string id);
    MachineDataModel? GetElementByModel(string model);
    void AddElement(MachineDataModel machineDataModel);
    void UpdElement(MachineDataModel machineDataModel);
    void DelElement(string id);
}