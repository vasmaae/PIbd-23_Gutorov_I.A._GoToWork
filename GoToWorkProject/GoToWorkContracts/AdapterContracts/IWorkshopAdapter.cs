using GoToWorkContracts.AdapterContracts.OperationResponses;
using GoToWorkContracts.BindingModels;

namespace GoToWorkContracts.AdapterContracts;

public interface IWorkshopAdapter
{
    WorkshopOperationResponse GetList();
    WorkshopOperationResponse GetListByProduction(string productionId);
    WorkshopOperationResponse GetElement(string data);
    WorkshopOperationResponse CreateWorkshop(WorkshopBindingModel workshopModel);
    WorkshopOperationResponse UpdateWorkshop(WorkshopBindingModel workshopModel);
    WorkshopOperationResponse DeleteWorkshop(string id);
}