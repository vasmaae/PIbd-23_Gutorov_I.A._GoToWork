using GoToWorkContracts.DataModels;

namespace GoToWorkContracts.StoragesContracts;

public interface IProductionStorageContract
{
    List<ProductionDataModel> GetList();
    ProductionDataModel? GetElementById(string id);
    ProductionDataModel? GetElementByName(string name);
    void AddElement(ProductionDataModel productionDataModel);
    void UpdElement(ProductionDataModel productionDataModel);
    void DelElement(string id);
}