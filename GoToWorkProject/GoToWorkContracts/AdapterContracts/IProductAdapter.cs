using GoToWorkContracts.AdapterContracts.OperationResponses;
using GoToWorkContracts.BindingModels;

namespace GoToWorkContracts.AdapterContracts;

public interface IProductAdapter
{
    ProductOperationResponse GetList();
    ProductOperationResponse GetListByMachine(string machineId);
    ProductOperationResponse GetListByCreationDate(DateTime from, DateTime to);
    ProductOperationResponse GetElement(string data);
    ProductOperationResponse CreateProduct(ProductBindingModel productModel);
    ProductOperationResponse UpdateProduct(ProductBindingModel productModel);
    ProductOperationResponse DeleteProduct(string id);
}