using GoToWorkContracts.AdapterContracts;
using GoToWorkContracts.BindingModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoToWorkApi.Controllers;

[AllowAnonymous]
[Route("api/[controller]/[action]")]
[ApiController]
[Produces("application/json")]
public class AuthController(IUserAdapter userAdapter) : ControllerBase
{
    [HttpPost]
    public IActionResult Register([FromBody] UserRegisterBindingModel model)
    {
        return userAdapter.Register(model).GetResponse(Request, Response);
    }

    [HttpPost]
    public IActionResult Login([FromBody] UserLoginBindingModel model)
    {
        return userAdapter.Login(model).GetResponse(Request, Response);
    }
}