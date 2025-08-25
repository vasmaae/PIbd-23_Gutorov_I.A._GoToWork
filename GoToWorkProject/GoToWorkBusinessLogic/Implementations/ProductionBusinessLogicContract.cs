using System.Text.Json;
using GoToWorkContracts.BusinessLogicContracts;
using GoToWorkContracts.DataModels;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.Extensions;
using GoToWorkContracts.StoragesContracts;
using Microsoft.Extensions.Logging;

namespace GoToWorkBusinessLogic.Implementations;

internal class ProductionBusinessLogicContract(
    IProductionStorageContract productionStorageContract,
    ILogger logger) : IProductionBusinessLogicContract
{
    public List<ProductionDataModel> GetAllProductions()
    {
        logger.LogInformation("Getting all productions");
        return productionStorageContract.GetList() ?? throw new NullListException();
    }
    
    public ProductionDataModel GetProductionByData(string data)
    {
        logger.LogInformation("Getting production by data: {data}", data);
        if (data.IsEmpty()) throw new ArgumentNullException(nameof(data));
        if (data.IsGuid())
            return productionStorageContract.GetElementById(data) ?? throw new ElementNotFoundException(data);
        throw new ElementNotFoundException(data);
    }

    public void InsertProduction(ProductionDataModel production)
    {
        logger.LogInformation("Inserting new production: {json}", JsonSerializer.Serialize(production));
        ArgumentNullException.ThrowIfNull(production);
        production.Validate();
        productionStorageContract.AddElement(production);
    }

    public void UpdateProduction(ProductionDataModel production)
    {
        logger.LogInformation("Updating production: {json}", JsonSerializer.Serialize(production));
        ArgumentNullException.ThrowIfNull(production);
        production.Validate();
        productionStorageContract.UpdElement(production);
    }

    public void DeleteProduction(string id)
    {
        logger.LogInformation("Deleting production with id: {id}", id);
        if (id.IsEmpty()) throw new ArgumentNullException(nameof(id));
        if (!id.IsGuid()) throw new ValidationException("Id is not a unique identifier");
        productionStorageContract.DelElement(id);
    }
}