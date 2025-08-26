using AutoMapper;
using GoToWorkContracts.AdapterContracts;
using GoToWorkContracts.AdapterContracts.OperationResponses;
using GoToWorkContracts.BindingModels;
using GoToWorkContracts.BusinessLogicContracts;
using GoToWorkContracts.DataModels;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.ViewModels;

namespace GoToWorkApi.Adapters;

public class ProductionAdapter : IProductionAdapter
{
    private readonly ILogger<ProductionAdapter> _logger;
    private readonly Mapper _mapper;
    private readonly IProductionBusinessLogicContract _productionBusinessLogic;

    public ProductionAdapter(IProductionBusinessLogicContract productionBusinessLogic,
        ILogger<ProductionAdapter> logger)
    {
        _productionBusinessLogic = productionBusinessLogic;
        _logger = logger;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ProductionBindingModel, ProductionDataModel>();
            cfg.CreateMap<DetailProductionBindingModel, DetailProductionDataModel>();
            cfg.CreateMap<ProductionDataModel, ProductionViewModel>();
            cfg.CreateMap<DetailProductionDataModel, DetailProductionViewModel>();
        });
        _mapper = new Mapper(config);
    }

    public ProductionOperationResponse GetList()
    {
        try
        {
            return ProductionOperationResponse.OK([
                .._productionBusinessLogic.GetAllProductions()
                    .Select(x => _mapper.Map<ProductionDataModel, ProductionViewModel>(x))
            ]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return ProductionOperationResponse.NotFound("The list is not initialized");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ProductionOperationResponse.InternalServerError(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ProductionOperationResponse.InternalServerError(ex.Message);
        }
    }

    public ProductionOperationResponse GetElement(string data)
    {
        try
        {
            return ProductionOperationResponse.OK(
                _mapper.Map<ProductionViewModel>(_productionBusinessLogic.GetProductionByData(data)));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ProductionOperationResponse.BadRequest("Data is empty");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ProductionOperationResponse.NotFound($"Not found element by data {data}");
        }
        catch (ElementDeletedException ex)
        {
            _logger.LogError(ex, "ElementDeletedException");
            return ProductionOperationResponse.BadRequest($"Element by data: {data} was deleted");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ProductionOperationResponse.InternalServerError(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ProductionOperationResponse.InternalServerError(ex.Message);
        }
    }

    public ProductionOperationResponse CreateProduction(ProductionBindingModel productionModel)
    {
        try
        {
            productionModel.Id = Guid.NewGuid().ToString();
            _productionBusinessLogic.InsertProduction(_mapper.Map<ProductionDataModel>(productionModel));
            return ProductionOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ProductionOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ProductionOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return ProductionOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ProductionOperationResponse.BadRequest(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ProductionOperationResponse.InternalServerError(ex.Message);
        }
    }

    public ProductionOperationResponse UpdateProduction(ProductionBindingModel productionModel)
    {
        try
        {
            _productionBusinessLogic.UpdateProduction(_mapper.Map<ProductionDataModel>(productionModel));
            return ProductionOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ProductionOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ProductionOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ProductionOperationResponse.BadRequest($"Not found element by Id {productionModel.Id}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return ProductionOperationResponse.BadRequest(ex.Message);
        }
        catch (ElementDeletedException ex)
        {
            _logger.LogError(ex, "ElementDeletedException");
            return ProductionOperationResponse.BadRequest($"Element by id: {productionModel.Id} was deleted");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ProductionOperationResponse.BadRequest(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ProductionOperationResponse.InternalServerError(ex.Message);
        }
    }

    public ProductionOperationResponse DeleteProduction(string id)
    {
        try
        {
            _productionBusinessLogic.DeleteProduction(id);
            return ProductionOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ProductionOperationResponse.BadRequest("Id is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ProductionOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ProductionOperationResponse.BadRequest($"Not found element by id: {id}");
        }
        catch (ElementDeletedException ex)
        {
            _logger.LogError(ex, "ElementDeletedException");
            return ProductionOperationResponse.BadRequest($"Element by id: {id} was deleted");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ProductionOperationResponse.BadRequest(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ProductionOperationResponse.InternalServerError(ex.Message);
        }
    }
}