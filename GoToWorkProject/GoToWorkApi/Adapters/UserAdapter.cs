using AutoMapper;
using GoToWorkContracts.AdapterContracts;
using GoToWorkContracts.AdapterContracts.OperationResponses;
using GoToWorkContracts.BindingModels;
using GoToWorkContracts.BusinessLogicContracts;
using GoToWorkContracts.DataModels;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GoToWorkApi.Adapters;

public class UserAdapter : IUserAdapter
{
    private readonly ILogger<UserAdapter> _logger;
    private readonly Mapper _mapper;
    private readonly IUserBusinessLogicContract _userBusinessLogic;

    public UserAdapter(IUserBusinessLogicContract userBusinessLogic, ILogger<UserAdapter> logger)
    {
        _userBusinessLogic = userBusinessLogic;
        _logger = logger;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserBindingModel, UserDataModel>();
            cfg.CreateMap<UserRegisterBindingModel, UserDataModel>();
            cfg.CreateMap<UserDataModel, UserViewModel>();
        });
        _mapper = new Mapper(config);
    }

    public UserOperationResponse GetList()
    {
        try
        {
            return UserOperationResponse.OK([
                .._userBusinessLogic.GetAllUsers()
                    .Select(x => _mapper.Map<UserDataModel, UserViewModel>(x))
            ]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return UserOperationResponse.NotFound("The list is not initialized");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return UserOperationResponse.InternalServerError(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return UserOperationResponse.InternalServerError(ex.Message);
        }
    }

    public UserOperationResponse GetElement(string data)
    {
        try
        {
            return UserOperationResponse.OK(
                _mapper.Map<UserViewModel>(_userBusinessLogic.GetUserByData(data)));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return UserOperationResponse.BadRequest("Data is empty");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return UserOperationResponse.NotFound($"Not found element by data {data}");
        }
        catch (ElementDeletedException ex)
        {
            _logger.LogError(ex, "ElementDeletedException");
            return UserOperationResponse.BadRequest($"Element by data: {data} was deleted");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return UserOperationResponse.InternalServerError(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return UserOperationResponse.InternalServerError(ex.Message);
        }
    }

    public UserOperationResponse CreateUser(UserBindingModel userModel)
    {
        try
        {
            _userBusinessLogic.InsertUser(_mapper.Map<UserDataModel>(userModel));
            return UserOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return UserOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return UserOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return UserOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return UserOperationResponse.BadRequest(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return UserOperationResponse.InternalServerError(ex.Message);
        }
    }

    public UserOperationResponse UpdateUser(UserBindingModel userModel)
    {
        try
        {
            _userBusinessLogic.UpdateUser(_mapper.Map<UserDataModel>(userModel));
            return UserOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return UserOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return UserOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return UserOperationResponse.BadRequest($"Not found element by Id {userModel.Id}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return UserOperationResponse.BadRequest(ex.Message);
        }
        catch (ElementDeletedException ex)
        {
            _logger.LogError(ex, "ElementDeletedException");
            return UserOperationResponse.BadRequest($"Element by id: {userModel.Id} was deleted");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return UserOperationResponse.BadRequest(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return UserOperationResponse.InternalServerError(ex.Message);
        }
    }

    public UserOperationResponse DeleteUser(string id)
    {
        try
        {
            _userBusinessLogic.DeleteUser(id);
            return UserOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return UserOperationResponse.BadRequest("Id is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return UserOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return UserOperationResponse.BadRequest($"Not found element by id: {id}");
        }
        catch (ElementDeletedException ex)
        {
            _logger.LogError(ex, "ElementDeletedException");
            return UserOperationResponse.BadRequest($"Element by id: {id} was deleted");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return UserOperationResponse.BadRequest(
                $"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return UserOperationResponse.InternalServerError(ex.Message);
        }
    }

    public UserOperationResponse Register(UserRegisterBindingModel model)
    {
        try
        {
            _userBusinessLogic.Register(_mapper.Map<UserDataModel>(model));
            return UserOperationResponse.NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return UserOperationResponse.InternalServerError(ex.Message);
        }
    }

    public AuthOperationResponse Login(UserLoginBindingModel model)
    {
        try
        {
            var user = _userBusinessLogic.Login(model.Login, model.Password);
            if (user == null)
            {
                return AuthOperationResponse.Unauthorized("Invalid login or password");
            }

            var claims = new List<Claim> {
                new(ClaimsIdentity.DefaultNameClaimType, user.Value.login),
                new(ClaimsIdentity.DefaultRoleClaimType, user.Value.role.ToString()),
                new("id", user.Value.id)
            };
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.Issuer,
                    audience: AuthOptions.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(60)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var tokenModel = new TokenViewModel { Token = encodedJwt };

            return AuthOperationResponse.OK(tokenModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return AuthOperationResponse.BadRequest(ex.Message);
        }
    }
}