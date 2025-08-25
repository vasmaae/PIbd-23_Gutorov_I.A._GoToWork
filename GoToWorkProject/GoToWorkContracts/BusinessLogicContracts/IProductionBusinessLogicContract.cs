using GoToWorkContracts.DataModels;

namespace GoToWorkContracts.BusinessLogicContracts;

public interface IProductionBusinessLogicContract
{
    List<ProductionDataModel> GetAllProductions();
    List<ProductionDataModel> GetProductionsByWorkshop(string workshopId);
    List<ProductionDataModel> GetProductionsByDetail(string detailId);
    ProductionDataModel GetProductionByData(string data);
    void InsertProduction(ProductionDataModel production);
    void UpdateProduction(ProductionDataModel production);
    void DeleteProduction(string id);
}