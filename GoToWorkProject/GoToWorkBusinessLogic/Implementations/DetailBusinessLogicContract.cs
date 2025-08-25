using System.Text.Json;
using GoToWorkContracts.BusinessLogicContracts;
using GoToWorkContracts.DataModels;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.Extensions;
using GoToWorkContracts.StoragesContracts;
using Microsoft.Extensions.Logging;

namespace GoToWorkBusinessLogic.Implementations;

public class DetailBusinessLogicContract(
    IDetailStorageContract detailStorageContract,
    ILogger logger) : IDetailBusinessLogicContract
{
    public List<DetailDataModel> GetAllDetails()
    {
        logger.LogInformation("Getting all details");
        return detailStorageContract.GetList() ?? throw new NullListException();
    }

    public List<DetailDataModel> GetDetailsByCreationDate(DateTime? from = null, DateTime? to = null)
    {
        logger.LogInformation("Getting details by creation date from {from} to {to}", from, to);
        if (from > to) throw new IncorrectDatesException((DateTime)from, (DateTime)to);
        return detailStorageContract.GetList(from, to) ?? throw new NullListException();
    }

    public DetailDataModel GetDetailByData(string data)
    {
        logger.LogInformation("Getting detail by data: {data}", data);
        if (data.IsEmpty()) throw new ArgumentNullException(nameof(data));
        if (data.IsGuid())
            return detailStorageContract.GetElementById(data) ?? throw new ElementNotFoundException(data);
        return detailStorageContract.GetElementByName(data) ?? throw new ElementNotFoundException(data);
    }

    public void InsertDetail(DetailDataModel detail)
    {
        logger.LogInformation("Inserting new detail: {json}", JsonSerializer.Serialize(detail));
        ArgumentNullException.ThrowIfNull(detail);
        detail.Validate();
        detailStorageContract.AddElement(detail);
    }

    public void UpdateDetail(DetailDataModel detail)
    {
        logger.LogInformation("Updating detail: {json}", JsonSerializer.Serialize(detail));
        ArgumentNullException.ThrowIfNull(detail);
        detail.Validate();
        detailStorageContract.UpdElement(detail);
    }

    public void DeleteDetail(string id)
    {
        logger.LogInformation("Deleting detail with id: {id}", id);
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));
        if (!id.IsGuid()) throw new ValidationException("Id is not a unique identifier");
        detailStorageContract.DelElement(id);
    }
}