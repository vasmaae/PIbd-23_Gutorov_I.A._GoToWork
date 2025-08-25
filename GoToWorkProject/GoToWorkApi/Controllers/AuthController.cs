using GoToWorkContracts.AdapterContracts;
using GoToWorkContracts.BindingModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoToWorkApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IUserAdapter _userAdapter;

        public AuthController(IUserAdapter userAdapter)
        {
            _userAdapter = userAdapter;
        }

        [HttpPost]
        public IActionResult Register([FromBody] UserRegisterBindingModel model)
        {
            return _userAdapter.Register(model).GetResponse(Request, Response);
        }

        [HttpPost]
        public IActionResult Login([FromBody] UserLoginBindingModel model)
        {
            return _userAdapter.Login(model).GetResponse(Request, Response);
        }
    }
}
