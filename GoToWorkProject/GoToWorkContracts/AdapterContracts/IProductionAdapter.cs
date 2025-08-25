using GoToWorkContracts.AdapterContracts.OperationResponses;
using GoToWorkContracts.BindingModels;

namespace GoToWorkContracts.AdapterContracts;

public interface IProductionAdapter
{
    ProductionOperationResponse GetList();
    ProductionOperationResponse GetElement(string data);
    ProductionOperationResponse CreateProduction(ProductionBindingModel productionModel);
    ProductionOperationResponse UpdateProduction(ProductionBindingModel productionModel);
    ProductionOperationResponse DeleteProduction(string id);
}