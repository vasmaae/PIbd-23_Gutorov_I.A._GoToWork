using System.Text.Json;
using GoToWorkContracts.BusinessLogicContracts;
using GoToWorkContracts.DataModels;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.Extensions;
using GoToWorkContracts.StoragesContracts;
using Microsoft.Extensions.Logging;

namespace GoToWorkBusinessLogic.Implementations;

public class WorkshopBusinessLogicContract(
    IWorkshopStorageContract workshopStorageContract,
    ILogger logger) : IWorkshopBusinessLogicContract
{
    public List<WorkshopDataModel> GetAllWorkshops()
    {
        logger.LogInformation("Getting all workshops");
        return workshopStorageContract.GetList() ?? throw new NullListException();
    }

    public List<WorkshopDataModel> GetWorkshopsByProduction(string productionId)
    {
        logger.LogInformation("Getting workshops by production ID: {productionId}", productionId);
        if (productionId.IsEmpty()) throw new ArgumentNullException(nameof(productionId));
        if (!productionId.IsGuid()) throw new ValidationException("ProductionId is not a unique identifier");
        return workshopStorageContract.GetList(productionId) ?? throw new NullListException();
    }

    public WorkshopDataModel GetWorkshopByData(string data)
    {
        logger.LogInformation("Getting workshop by data: {data}", data);
        if (data.IsEmpty()) throw new ArgumentNullException(nameof(data));
        if (data.IsGuid())
            return workshopStorageContract.GetElementById(data) ?? throw new ElementNotFoundException(data);
        return workshopStorageContract.GetElementByAddress(data) ?? throw new ElementNotFoundException(data);
    }

    public void InsertWorkshop(WorkshopDataModel workshop)
    {
        logger.LogInformation("Inserting new workshop: {json}", JsonSerializer.Serialize(workshop));
        ArgumentNullException.ThrowIfNull(workshop);
        workshop.Validate();
        workshopStorageContract.AddElement(workshop);
    }

    public void UpdateWorkshop(WorkshopDataModel workshop)
    {
        logger.LogInformation("Updating workshop: {json}", JsonSerializer.Serialize(workshop));
        ArgumentNullException.ThrowIfNull(workshop);
        workshop.Validate();
        workshopStorageContract.UpdElement(workshop);
    }

    public void DeleteWorkshop(string id)
    {
        logger.LogInformation("Deleting workshop with Id: {id}", id);
        if (id.IsEmpty()) throw new ArgumentNullException(nameof(id));
        if (!id.IsGuid()) throw new ValidationException("Id is not a unique identifier");
        workshopStorageContract.DelElement(id);
    }
}