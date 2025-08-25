using GoToWorkContracts.DataModels;

namespace GoToWorkContracts.StoragesContracts;

public interface IWorkshopStorageContract
{
    List<WorkshopDataModel> GetList(string? productionId = null);
    WorkshopDataModel? GetElementById(string id);
    public WorkshopDataModel? GetElementByAddress(string address);
    void AddElement(WorkshopDataModel workshopDataModel);
    void UpdElement(WorkshopDataModel workshopDataModel);
    void DelElement(string id);
}