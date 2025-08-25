using GoToWorkContracts.DataModels;

namespace GoToWorkContracts.StoragesContracts;

public interface IWorkshopStorageContract
{
    List<WorkshopDataModel> GetList(string? employeeId = null);
    WorkshopDataModel? GetElementById(string id);
    List<WorkshopDataModel> GetElementsByAddress(string address);
    void AddElement(WorkshopDataModel workshopDataModel);
    void UpdElement(WorkshopDataModel workshopDataModel);
    void DelElement(string id);
}