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

    public List<ProductionDataModel> GetProductionsByWorkshop(string workshopId)
    {
        logger.LogInformation("Getting productions by workshop: {workshopId}", workshopId);
        if (workshopId.IsEmpty()) throw new ArgumentNullException(nameof(workshopId));
        if (!workshopId.IsGuid()) throw new ValidationException("WorkshopId is not a unique identifier");
        return productionStorageContract.GetList(workshopId)
               ?? throw new NullListException();
    }

    public List<ProductionDataModel> GetProductionsByDetail(string detailId)
    {
        logger.LogInformation("Getting productions by detail: {detailId}", detailId);
        if (detailId.IsEmpty()) throw new ArgumentNullException(nameof(detailId));
        if (!detailId.IsGuid()) throw new ValidationException("DetailId is not a unique identifier");
        return productionStorageContract.GetList(detailId: detailId)
               ?? throw new NullListException();
    }

    public ProductionDataModel? GetProductionByData(string data)
    {
        logger.LogInformation("Getting production by data: {data}", data);
        if (data.IsEmpty()) throw new ArgumentNullException(nameof(data));
        if (data.IsGuid())
            return productionStorageContract.GetElementById(data)
                   ?? throw new ElementNotFoundException(data);
        throw new NullListException();
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