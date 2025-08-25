using GoToWorkContracts.DataModels;

namespace GoToWorkContracts.StoragesContracts;

public interface IProductionStorageContract
{
    List<ProductionDataModel> GetList(string? workshopId = null, string? detailId = null);
    ProductionDataModel? GetElementById(string id);
    void AddElement(ProductionDataModel productionDataModel);
    void UpdElement(ProductionDataModel productionDataModel);
    void DelElement(string id);
}