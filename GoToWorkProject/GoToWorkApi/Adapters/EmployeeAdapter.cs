using AutoMapper;
using GoToWorkContracts.AdapterContracts;
using GoToWorkContracts.AdapterContracts.OperationResponses;
using GoToWorkContracts.BindingModels;
using GoToWorkContracts.BusinessLogicContracts;
using GoToWorkContracts.DataModels;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.ViewModels;

namespace GoToWorkApi.Adapters;

public class EmployeeAdapter : IEmployeeAdapter
{
    private readonly IEmployeeBusinessLogicContract _employeeBusinessLogic;
    private readonly ILogger<EmployeeAdapter> _logger;
    private readonly Mapper _mapper;

    public EmployeeAdapter(IEmployeeBusinessLogicContract employeeBusinessLogic, ILogger<EmployeeAdapter> logger)
    {
        _employeeBusinessLogic = employeeBusinessLogic;
        _logger = logger;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<EmployeeBindingModel, EmployeeDataModel>();
            cfg.CreateMap<EmployeeDataModel, EmployeeViewModel>();
        });
        _mapper = new Mapper(config);
    }

    public EmployeeOperationResponse GetList()
    {
        try
        {
            return EmployeeOperationResponse.OK([
                .._employeeBusinessLogic.GetAllEmployees()
                    .Select(x => _mapper.Map<EmployeeDataModel, EmployeeViewModel>(x))
            ]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return EmployeeOperationResponse.NotFound("The list is not initialized");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return EmployeeOperationResponse.InternalServerError(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return EmployeeOperationResponse.InternalServerError(ex.Message);
        }
    }

    public EmployeeOperationResponse GetElement(string data)
    {
        try
        {
            return EmployeeOperationResponse.OK(
                _mapper.Map<EmployeeViewModel>(_employeeBusinessLogic.GetEmployeeByData(data)));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return EmployeeOperationResponse.BadRequest("Data is empty");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return EmployeeOperationResponse.NotFound($"Not found element by data {data}");
        }
        catch (ElementDeletedException ex)
        {
            _logger.LogError(ex, "ElementDeletedException");
            return EmployeeOperationResponse.BadRequest($"Element by data: {data} was deleted");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return EmployeeOperationResponse.InternalServerError(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return EmployeeOperationResponse.InternalServerError(ex.Message);
        }
    }

    public EmployeeOperationResponse CreateEmployee(EmployeeBindingModel employeeModel)
    {
        try
        {
            employeeModel.Id = Guid.NewGuid().ToString();
            _employeeBusinessLogic.InsertEmployee(_mapper.Map<EmployeeDataModel>(employeeModel));
            return EmployeeOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return EmployeeOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return EmployeeOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return EmployeeOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return EmployeeOperationResponse.BadRequest(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return EmployeeOperationResponse.InternalServerError(ex.Message);
        }
    }

    public EmployeeOperationResponse UpdateEmployee(EmployeeBindingModel employeeModel)
    {
        try
        {
            _employeeBusinessLogic.UpdateEmployee(_mapper.Map<EmployeeDataModel>(employeeModel));
            return EmployeeOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return EmployeeOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return EmployeeOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return EmployeeOperationResponse.BadRequest($"Not found element by Id {employeeModel.Id}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return EmployeeOperationResponse.BadRequest(ex.Message);
        }
        catch (ElementDeletedException ex)
        {
            _logger.LogError(ex, "ElementDeletedException");
            return EmployeeOperationResponse.BadRequest($"Element by id: {employeeModel.Id} was deleted");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return EmployeeOperationResponse.BadRequest(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return EmployeeOperationResponse.InternalServerError(ex.Message);
        }
    }

    public EmployeeOperationResponse DeleteEmployee(string id)
    {
        try
        {
            _employeeBusinessLogic.DeleteEmployee(id);
            return EmployeeOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return EmployeeOperationResponse.BadRequest("Id is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return EmployeeOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return EmployeeOperationResponse.BadRequest($"Not found element by id: {id}");
        }
        catch (ElementDeletedException ex)
        {
            _logger.LogError(ex, "ElementDeletedException");
            return EmployeeOperationResponse.BadRequest($"Element by id: {id} was deleted");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return EmployeeOperationResponse.BadRequest(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return EmployeeOperationResponse.InternalServerError(ex.Message);
        }
    }
}