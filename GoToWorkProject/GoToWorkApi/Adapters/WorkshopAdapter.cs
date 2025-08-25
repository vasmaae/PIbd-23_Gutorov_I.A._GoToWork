using AutoMapper;
using GoToWorkContracts.AdapterContracts;
using GoToWorkContracts.AdapterContracts.OperationResponses;
using GoToWorkContracts.BindingModels;
using GoToWorkContracts.BusinessLogicContracts;
using GoToWorkContracts.DataModels;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.ViewModels;

namespace GoToWorkApi.Adapters;

public class WorkshopAdapter : IWorkshopAdapter
{
    private readonly ILogger<WorkshopAdapter> _logger;
    private readonly Mapper _mapper;
    private readonly IWorkshopBusinessLogicContract _workshopBusinessLogic;

    public WorkshopAdapter(IWorkshopBusinessLogicContract workshopBusinessLogic, ILogger<WorkshopAdapter> logger)
    {
        _workshopBusinessLogic = workshopBusinessLogic;
        _logger = logger;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<WorkshopBindingModel, WorkshopDataModel>();
            cfg.CreateMap<EmployeeWorkshopBindingModel, EmployeeWorkshopDataModel>();
            cfg.CreateMap<WorkshopDataModel, WorkshopViewModel>();
            cfg.CreateMap<EmployeeWorkshopDataModel, EmployeeWorkshopViewModel>();
        });
        _mapper = new Mapper(config);
    }

    public WorkshopOperationResponse GetList()
    {
        try
        {
            return WorkshopOperationResponse.OK([
                .._workshopBusinessLogic.GetAllWorkshops()
                    .Select(x => _mapper.Map<WorkshopDataModel, WorkshopViewModel>(x))
            ]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return WorkshopOperationResponse.NotFound("The list is not initialized");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkshopOperationResponse.InternalServerError(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkshopOperationResponse.InternalServerError(ex.Message);
        }
    }

    public WorkshopOperationResponse GetListByProduction(string productionId)
    {
        try
        {
            return WorkshopOperationResponse.OK([
                .._workshopBusinessLogic.GetWorkshopsByProduction(productionId)
                    .Select(x => _mapper.Map<WorkshopDataModel, WorkshopViewModel>(x))
            ]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return WorkshopOperationResponse.NotFound("The list is not initialized");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkshopOperationResponse.InternalServerError(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkshopOperationResponse.InternalServerError(ex.Message);
        }
    }

    public WorkshopOperationResponse GetElement(string data)
    {
        try
        {
            return WorkshopOperationResponse.OK(
                _mapper.Map<WorkshopViewModel>(_workshopBusinessLogic.GetWorkshopByData(data)));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return WorkshopOperationResponse.BadRequest("Data is empty");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return WorkshopOperationResponse.NotFound($"Not found element by data {data}");
        }
        catch (ElementDeletedException ex)
        {
            _logger.LogError(ex, "ElementDeletedException");
            return WorkshopOperationResponse.BadRequest($"Element by data: {data} was deleted");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkshopOperationResponse.InternalServerError(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkshopOperationResponse.InternalServerError(ex.Message);
        }
    }

    public WorkshopOperationResponse CreateWorkshop(WorkshopBindingModel workshopModel)
    {
        try
        {
            _workshopBusinessLogic.InsertWorkshop(_mapper.Map<WorkshopDataModel>(workshopModel));
            return WorkshopOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return WorkshopOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return WorkshopOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return WorkshopOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkshopOperationResponse.BadRequest(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkshopOperationResponse.InternalServerError(ex.Message);
        }
    }

    public WorkshopOperationResponse UpdateWorkshop(WorkshopBindingModel workshopModel)
    {
        try
        {
            _workshopBusinessLogic.UpdateWorkshop(_mapper.Map<WorkshopDataModel>(workshopModel));
            return WorkshopOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return WorkshopOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return WorkshopOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return WorkshopOperationResponse.BadRequest($"Not found element by Id {workshopModel.Id}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return WorkshopOperationResponse.BadRequest(ex.Message);
        }
        catch (ElementDeletedException ex)
        {
            _logger.LogError(ex, "ElementDeletedException");
            return WorkshopOperationResponse.BadRequest($"Element by id: {workshopModel.Id} was deleted");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkshopOperationResponse.BadRequest(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkshopOperationResponse.InternalServerError(ex.Message);
        }
    }

    public WorkshopOperationResponse DeleteWorkshop(string id)
    {
        try
        {
            _workshopBusinessLogic.DeleteWorkshop(id);
            return WorkshopOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return WorkshopOperationResponse.BadRequest("Id is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return WorkshopOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return WorkshopOperationResponse.BadRequest($"Not found element by id: {id}");
        }
        catch (ElementDeletedException ex)
        {
            _logger.LogError(ex, "ElementDeletedException");
            return WorkshopOperationResponse.BadRequest($"Element by id: {id} was deleted");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkshopOperationResponse.BadRequest(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkshopOperationResponse.InternalServerError(ex.Message);
        }
    }
}