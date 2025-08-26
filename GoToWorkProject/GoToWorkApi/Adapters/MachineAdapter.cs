using AutoMapper;
using GoToWorkContracts.AdapterContracts;
using GoToWorkContracts.AdapterContracts.OperationResponses;
using GoToWorkContracts.BindingModels;
using GoToWorkContracts.BusinessLogicContracts;
using GoToWorkContracts.DataModels;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.ViewModels;

namespace GoToWorkApi.Adapters;

public class MachineAdapter : IMachineAdapter
{
    private readonly ILogger<MachineAdapter> _logger;
    private readonly IMachineBusinessLogicContract _machineBusinessLogic;
    private readonly Mapper _mapper;

    public MachineAdapter(IMachineBusinessLogicContract machineBusinessLogic, ILogger<MachineAdapter> logger)
    {
        _machineBusinessLogic = machineBusinessLogic;
        _logger = logger;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<MachineBindingModel, MachineDataModel>();
            cfg.CreateMap<MachineDataModel, MachineViewModel>();
            cfg.CreateMap<ProductBindingModel, ProductDataModel>();
            cfg.CreateMap<ProductDataModel, ProductViewModel>();
            cfg.CreateMap<EmployeeBindingModel, EmployeeDataModel>();
            cfg.CreateMap<EmployeeDataModel, EmployeeViewModel>();
            cfg.CreateMap<EmployeeMachineBindingModel, EmployeeMachineDataModel>();
            cfg.CreateMap<EmployeeMachineDataModel, EmployeeMachineViewModel>();
        });
        _mapper = new Mapper(config);
    }

    public MachineOperationResponse GetList()
    {
        try
        {
            return MachineOperationResponse.OK([
                .._machineBusinessLogic.GetAllMachines()
                    .Select(x => _mapper.Map<MachineDataModel, MachineViewModel>(x))
            ]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return MachineOperationResponse.NotFound("The list is not initialized");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return MachineOperationResponse.InternalServerError(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return MachineOperationResponse.InternalServerError(ex.Message);
        }
    }

    public MachineOperationResponse GetElement(string data)
    {
        try
        {
            return MachineOperationResponse.OK(
                _mapper.Map<MachineViewModel>(_machineBusinessLogic.GetMachineByData(data)));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return MachineOperationResponse.BadRequest("Data is empty");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return MachineOperationResponse.NotFound($"Not found element by data {data}");
        }
        catch (ElementDeletedException ex)
        {
            _logger.LogError(ex, "ElementDeletedException");
            return MachineOperationResponse.BadRequest($"Element by data: {data} was deleted");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return MachineOperationResponse.InternalServerError(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return MachineOperationResponse.InternalServerError(ex.Message);
        }
    }

    public MachineOperationResponse CreateMachine(MachineBindingModel machineModel)
    {
        try
        {
            machineModel.Id = Guid.NewGuid().ToString();
            _machineBusinessLogic.InsertMachine(_mapper.Map<MachineDataModel>(machineModel));
            return MachineOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return MachineOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return MachineOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return MachineOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return MachineOperationResponse.BadRequest(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return MachineOperationResponse.InternalServerError(ex.Message);
        }
    }

    public MachineOperationResponse UpdateMachine(MachineBindingModel machineModel)
    {
        try
        {
            _machineBusinessLogic.UpdateMachine(_mapper.Map<MachineDataModel>(machineModel));
            return MachineOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return MachineOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return MachineOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return MachineOperationResponse.BadRequest($"Not found element by Id {machineModel.Id}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return MachineOperationResponse.BadRequest(ex.Message);
        }
        catch (ElementDeletedException ex)
        {
            _logger.LogError(ex, "ElementDeletedException");
            return MachineOperationResponse.BadRequest($"Element by id: {machineModel.Id} was deleted");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return MachineOperationResponse.BadRequest(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return MachineOperationResponse.InternalServerError(ex.Message);
        }
    }

    public MachineOperationResponse DeleteMachine(string id)
    {
        try
        {
            _machineBusinessLogic.DeleteMachine(id);
            return MachineOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return MachineOperationResponse.BadRequest("Id is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return MachineOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return MachineOperationResponse.BadRequest($"Not found element by id: {id}");
        }
        catch (ElementDeletedException ex)
        {
            _logger.LogError(ex, "ElementDeletedException");
            return MachineOperationResponse.BadRequest($"Element by id: {id} was deleted");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return MachineOperationResponse.BadRequest(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return MachineOperationResponse.InternalServerError(ex.Message);
        }
    }
}