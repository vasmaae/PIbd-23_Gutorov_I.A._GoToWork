using GoToWorkContracts.DataModels;

namespace GoToWorkContracts.BusinessLogicContracts;

public interface IProductBusinessLogicContract
{
    List<ProductDataModel> GetAllProducts();
    List<ProductDataModel> GetProductsByMachine(string machineId);
    List<ProductDataModel> GetProductsByCreationDate(DateTime from, DateTime to);
    List<ProductDataModel> GetProductsByDetail(string detailId);
    ProductDataModel GetProductByData(string data);
    void InsertProduct(ProductDataModel product);
    void UpdateProduct(ProductDataModel product);
    void DeleteProduct(string id);
    void AddDetailToProduct(string productId, DetailProductDataModel detail);
    void RemoveDetailFromProduct(string productId, string detailId);
}