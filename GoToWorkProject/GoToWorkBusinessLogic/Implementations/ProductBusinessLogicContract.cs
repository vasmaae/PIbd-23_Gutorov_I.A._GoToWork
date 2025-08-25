using System.Text.Json;
using GoToWorkContracts.BusinessLogicContracts;
using GoToWorkContracts.DataModels;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.Extensions;
using GoToWorkContracts.StoragesContracts;
using Microsoft.Extensions.Logging;

namespace GoToWorkBusinessLogic.Implementations;

public class ProductBusinessLogicContract(
    IProductStorageContract productStorageContract,
    ILogger logger) : IProductBusinessLogicContract
{
    public List<ProductDataModel> GetAllProducts()
    {
        logger.LogInformation("Getting all products");
        return productStorageContract.GetList() ?? throw new NullListException();
    }

    public List<ProductDataModel> GetProductsByMachine(string machineId)
    {
        logger.LogInformation("Getting products by machine: {machineId}", machineId);
        if (machineId.IsEmpty()) throw new ArgumentNullException(nameof(machineId));
        if (!machineId.IsGuid()) throw new ValidationException("MachineId is not a unique identifier");
        return productStorageContract.GetList(machineId: machineId) ?? throw new NullListException();
    }

    public List<ProductDataModel> GetProductsByCreationDate(DateTime from, DateTime to)
    {
        logger.LogInformation("Getting products by date range: {from} - {to}", from, to);
        if (from > to) throw new ValidationException("Start date cannot be later than end date");
        return productStorageContract.GetList(from, to) ?? throw new NullListException();
    }

    public ProductDataModel GetProductByData(string data)
    {
        logger.LogInformation("Getting product by data: {data}", data);
        if (data.IsEmpty()) throw new ArgumentNullException(nameof(data));
        if (data.IsGuid())
            return productStorageContract.GetElementById(data) ?? throw new ElementNotFoundException(data);
        return productStorageContract.GetElementByName(data) ?? throw new ElementNotFoundException(data);
    }

    public void InsertProduct(ProductDataModel product)
    {
        logger.LogInformation("Inserting new product: {json}", JsonSerializer.Serialize(product));
        ArgumentNullException.ThrowIfNull(product);
        product.Validate();
        productStorageContract.AddElement(product);
    }

    public void UpdateProduct(ProductDataModel product)
    {
        logger.LogInformation("Updating product: {json}", JsonSerializer.Serialize(product));
        ArgumentNullException.ThrowIfNull(product);
        product.Validate();
        productStorageContract.UpdElement(product);
    }

    public void DeleteProduct(string id)
    {
        logger.LogInformation("Deleting product with id: {id}", id);
        if (id.IsEmpty()) throw new ArgumentNullException(nameof(id));
        if (!id.IsGuid()) throw new ValidationException("Id is not a unique identifier");
        productStorageContract.DelElement(id);
    }
}