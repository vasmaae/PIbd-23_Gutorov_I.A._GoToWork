using AutoMapper;
using GoToWorkContracts.AdapterContracts;
using GoToWorkContracts.AdapterContracts.OperationResponses;
using GoToWorkContracts.BindingModels;
using GoToWorkContracts.BusinessLogicContracts;
using GoToWorkContracts.DataModels;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.ViewModels;

namespace GoToWorkApi.Adapters;

public class DetailAdapter : IDetailAdapter
{
    private readonly IDetailBusinessLogicContract _detailBusinessLogicContract;
    private readonly ILogger _logger;
    private readonly Mapper _mapper;

    public DetailAdapter(IDetailBusinessLogicContract detailBusinessLogicContract, ILogger logger)
    {
        _detailBusinessLogicContract = detailBusinessLogicContract;
        _logger = logger;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DetailBindingModel, DetailDataModel>();
            cfg.CreateMap<DetailDataModel, DetailViewModel>();
        });
        _mapper = new Mapper(config);
    }

    public DetailOperationResponse GetList()
    {
        try
        {
            return DetailOperationResponse.OK([
                .._detailBusinessLogicContract.GetAllDetails()
                    .Select(x => _mapper.Map<DetailDataModel, DetailViewModel>(x))
            ]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return DetailOperationResponse.NotFound("The list is not initialized");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return DetailOperationResponse.InternalServerError(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return DetailOperationResponse.InternalServerError(ex.Message);
        }
    }

    public DetailOperationResponse GetElementsByCreationDate(DateTime? from = null, DateTime? to = null)
    {
        try
        {
            return DetailOperationResponse.OK([
                .._detailBusinessLogicContract.GetDetailsByCreationDate(from, to)
                    .Select(x => _mapper.Map<DetailDataModel, DetailViewModel>(x))
            ]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return DetailOperationResponse.NotFound("The list is not initialized");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return DetailOperationResponse.InternalServerError(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return DetailOperationResponse.InternalServerError(ex.Message);
        }
    }

    public DetailOperationResponse GetElement(string data)
    {
        try
        {
            return DetailOperationResponse.OK(
                _mapper.Map<DetailViewModel>(_detailBusinessLogicContract.GetDetailByData(data)));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return DetailOperationResponse.BadRequest("Data is empty");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return DetailOperationResponse.NotFound($"Not found element by data {data}");
        }
        catch (ElementDeletedException ex)
        {
            _logger.LogError(ex, "ElementDeletedException");
            return DetailOperationResponse.BadRequest($"Element by data: {data} was deleted");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return DetailOperationResponse.InternalServerError(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return DetailOperationResponse.InternalServerError(ex.Message);
        }
    }

    public DetailOperationResponse RegisterDetail(DetailBindingModel detailModel)
    {
        try
        {
            detailModel.Id = Guid.NewGuid().ToString();
            _detailBusinessLogicContract.InsertDetail(_mapper.Map<DetailDataModel>(detailModel));
            return DetailOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return DetailOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return DetailOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return DetailOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return DetailOperationResponse.BadRequest(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return DetailOperationResponse.InternalServerError(ex.Message);
        }
    }

    public DetailOperationResponse ChangeDetailInfo(DetailBindingModel detailModel)
    {
        try
        {
            _detailBusinessLogicContract.UpdateDetail(_mapper.Map<DetailDataModel>(detailModel));
            return DetailOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return DetailOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return DetailOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return DetailOperationResponse.BadRequest($"Not found element by Id {detailModel.Id}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return DetailOperationResponse.BadRequest(ex.Message);
        }
        catch (ElementDeletedException ex)
        {
            _logger.LogError(ex, "ElementDeletedException");
            return DetailOperationResponse.BadRequest($"Element by id: {detailModel.Id} was deleted");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return DetailOperationResponse.BadRequest(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return DetailOperationResponse.InternalServerError(ex.Message);
        }
    }

    public DetailOperationResponse RemoveDetail(string id)
    {
        try
        {
            _detailBusinessLogicContract.DeleteDetail(id);
            return DetailOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return DetailOperationResponse.BadRequest("Id is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return DetailOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return DetailOperationResponse.BadRequest($"Not found element by id: {id}");
        }
        catch (ElementDeletedException ex)
        {
            _logger.LogError(ex, "ElementDeletedException");
            return DetailOperationResponse.BadRequest($"Element by id: {id} was deleted");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return DetailOperationResponse.BadRequest(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return DetailOperationResponse.InternalServerError(ex.Message);
        }
    }
}