using GoToWorkContracts.DataModels;

namespace GoToWorkContracts.StoragesContracts;

public interface IProductStorageContract
{
    List<ProductDataModel> GetList(DateTime? startDate = null, DateTime? endDate = null, string? machineId = null);
    ProductDataModel? GetElementById(string id);
    ProductDataModel? GetElementByName(string name);
    void AddElement(ProductDataModel productDataModel);
    void UpdElement(ProductDataModel productDataModel);
    void DelElement(string id);
}