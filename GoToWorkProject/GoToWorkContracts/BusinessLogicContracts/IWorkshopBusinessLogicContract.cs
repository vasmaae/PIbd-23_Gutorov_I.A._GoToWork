using GoToWorkContracts.DataModels;

namespace GoToWorkContracts.BusinessLogicContracts;

public interface IWorkshopBusinessLogicContract
{
    List<WorkshopDataModel> GetAllWorkshops();
    List<WorkshopDataModel> GetWorkshopsByEmployee(string employeeId);
    List<WorkshopDataModel> GetWorkshopsByProduction(string productionId);
    WorkshopDataModel GetWorkshopByData(string data);
    void InsertWorkshop(WorkshopDataModel workshop);
    void UpdateWorkshop(WorkshopDataModel workshop);
    void DeleteWorkshop(string id);
}