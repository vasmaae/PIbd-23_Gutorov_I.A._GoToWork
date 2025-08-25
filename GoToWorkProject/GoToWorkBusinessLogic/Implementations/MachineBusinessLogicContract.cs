using System.Text.Json;
using GoToWorkContracts.BusinessLogicContracts;
using GoToWorkContracts.DataModels;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.Extensions;
using GoToWorkContracts.StoragesContracts;
using Microsoft.Extensions.Logging;

namespace GoToWorkBusinessLogic.Implementations;

public class MachineBusinessLogicContract(
    IMachineStorageContract machineStorageContract,
    ILogger logger) : IMachineBusinessLogicContract
{
    public List<MachineDataModel> GetAllMachines()
    {
        logger.LogInformation("Getting all machines");
        return machineStorageContract.GetList() ?? throw new NullListException();
    }

    public MachineDataModel GetMachineByData(string data)
    {
        logger.LogInformation("Getting machine by data: {data}", data);
        if (data.IsEmpty()) throw new ArgumentNullException(nameof(data));
        if (data.IsGuid())
            return machineStorageContract.GetElementById(data) ?? throw new ElementNotFoundException(data);
        return machineStorageContract.GetElementByModel(data) ?? throw new ElementNotFoundException(data);
    }

    public void InsertMachine(MachineDataModel machine)
    {
        logger.LogInformation("Inserting new machine: {json}", JsonSerializer.Serialize(machine));
        ArgumentNullException.ThrowIfNull(machine);
        machine.Validate();
        machineStorageContract.AddElement(machine);
    }

    public void UpdateMachine(MachineDataModel machine)
    {
        logger.LogInformation("Updating machine: {json}", JsonSerializer.Serialize(machine));
        ArgumentNullException.ThrowIfNull(machine);
        machine.Validate();
        machineStorageContract.UpdElement(machine);
    }

    public void DeleteMachine(string id)
    {
        logger.LogInformation("Deleting machine with id: {id}", id);
        if (id.IsEmpty()) throw new ArgumentNullException(nameof(id));
        if (!id.IsGuid()) throw new ValidationException("Id is not a unique identifier");
        machineStorageContract.DelElement(id);
    }
}