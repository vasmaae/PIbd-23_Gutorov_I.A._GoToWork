using AutoMapper;
using GoToWorkContracts.AdapterContracts;
using GoToWorkContracts.AdapterContracts.OperationResponses;
using GoToWorkContracts.BindingModels;
using GoToWorkContracts.BusinessLogicContracts;
using GoToWorkContracts.DataModels;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.ViewModels;

namespace GoToWorkApi.Adapters;

public class ProductAdapter : IProductAdapter
{
    private readonly ILogger<ProductAdapter> _logger;
    private readonly Mapper _mapper;
    private readonly IProductBusinessLogicContract _productBusinessLogic;

    public ProductAdapter(IProductBusinessLogicContract productBusinessLogic, ILogger<ProductAdapter> logger)
    {
        _productBusinessLogic = productBusinessLogic;
        _logger = logger;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DetailProductBindingModel, DetailProductDataModel>();
            cfg.CreateMap<DetailProductDataModel, DetailProductViewModel>();
            cfg.CreateMap<ProductBindingModel, ProductDataModel>();
            cfg.CreateMap<ProductDataModel, ProductViewModel>() ;
        });
        _mapper = new Mapper(config);
    }

    public ProductOperationResponse GetList()
    {
        try
        {
            return ProductOperationResponse.OK([
                .._productBusinessLogic.GetAllProducts()
                    .Select(x => _mapper.Map<ProductDataModel, ProductViewModel>(x))
            ]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return ProductOperationResponse.NotFound("The list is not initialized");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ProductOperationResponse.InternalServerError(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ProductOperationResponse.InternalServerError(ex.Message);
        }
    }

    public ProductOperationResponse GetListByMachine(string machineId)
    {
        try
        {
            return ProductOperationResponse.OK([
                .._productBusinessLogic.GetProductsByMachine(machineId)
                    .Select(x => _mapper.Map<ProductDataModel, ProductViewModel>(x))
            ]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return ProductOperationResponse.NotFound("The list is not initialized");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ProductOperationResponse.InternalServerError(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ProductOperationResponse.InternalServerError(ex.Message);
        }
    }

    public ProductOperationResponse GetListByCreationDate(DateTime from, DateTime to)
    {
        try
        {
            return ProductOperationResponse.OK([
                .._productBusinessLogic.GetProductsByCreationDate(from, to)
                    .Select(x => _mapper.Map<ProductDataModel, ProductViewModel>(x))
            ]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return ProductOperationResponse.NotFound("The list is not initialized");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ProductOperationResponse.InternalServerError(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ProductOperationResponse.InternalServerError(ex.Message);
        }
    }

    public ProductOperationResponse GetElement(string data)
    {
        try
        {
            return ProductOperationResponse.OK(
                _mapper.Map<ProductViewModel>(_productBusinessLogic.GetProductByData(data)));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ProductOperationResponse.BadRequest("Data is empty");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ProductOperationResponse.NotFound($"Not found element by data {data}");
        }
        catch (ElementDeletedException ex)
        {
            _logger.LogError(ex, "ElementDeletedException");
            return ProductOperationResponse.BadRequest($"Element by data: {data} was deleted");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ProductOperationResponse.InternalServerError(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ProductOperationResponse.InternalServerError(ex.Message);
        }
    }

    public ProductOperationResponse CreateProduct(ProductBindingModel productModel)
    {
        try
        {
            productModel.Id = Guid.NewGuid().ToString();
            _productBusinessLogic.InsertProduct(_mapper.Map<ProductDataModel>(productModel));
            return ProductOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ProductOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ProductOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return ProductOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ProductOperationResponse.BadRequest(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ProductOperationResponse.InternalServerError(ex.Message);
        }
    }

    public ProductOperationResponse UpdateProduct(ProductBindingModel productModel)
    {
        try
        {
            _productBusinessLogic.UpdateProduct(_mapper.Map<ProductDataModel>(productModel));
            return ProductOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ProductOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ProductOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ProductOperationResponse.BadRequest($"Not found element by Id {productModel.Id}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return ProductOperationResponse.BadRequest(ex.Message);
        }
        catch (ElementDeletedException ex)
        {
            _logger.LogError(ex, "ElementDeletedException");
            return ProductOperationResponse.BadRequest($"Element by id: {productModel.Id} was deleted");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ProductOperationResponse.BadRequest(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ProductOperationResponse.InternalServerError(ex.Message);
        }
    }

    public ProductOperationResponse DeleteProduct(string id)
    {
        try
        {
            _productBusinessLogic.DeleteProduct(id);
            return ProductOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ProductOperationResponse.BadRequest("Id is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ProductOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ProductOperationResponse.BadRequest($"Not found element by id: {id}");
        }
        catch (ElementDeletedException ex)
        {
            _logger.LogError(ex, "ElementDeletedException");
            return ProductOperationResponse.BadRequest($"Element by id: {id} was deleted");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ProductOperationResponse.BadRequest(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ProductOperationResponse.InternalServerError(ex.Message);
        }
    }
}