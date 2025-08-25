using GoToWorkContracts.DataModels;

namespace GoToWorkContracts.BusinessLogicContracts;

public interface IProductBusinessLogicContract
{
    List<ProductDataModel> GetAllProducts();
    List<ProductDataModel> GetProductsByMachine(string machineId);
    List<ProductDataModel> GetProductsByCreationDate(DateTime from, DateTime to);
    ProductDataModel GetProductByData(string data);
    void InsertProduct(ProductDataModel product);
    void UpdateProduct(ProductDataModel product);
    void DeleteProduct(string id);
}