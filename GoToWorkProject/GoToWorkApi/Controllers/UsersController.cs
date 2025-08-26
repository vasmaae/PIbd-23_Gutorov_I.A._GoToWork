using GoToWorkContracts.AdapterContracts;
using GoToWorkContracts.BindingModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoToWorkApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class UsersController(IUserAdapter adapter) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllRecords()
    {
        return adapter.GetList().GetResponse(Request, Response);
    }

    [HttpGet("{data}")]
    public IActionResult GetRecord(string data)
    {
        return adapter.GetElement(data).GetResponse(Request, Response);
    }

    [HttpPut]
    public IActionResult Update([FromBody] UserBindingModel model)
    {
        return adapter.UpdateUser(model).GetResponse(Request, Response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        return adapter.DeleteUser(id).GetResponse(Request, Response);
    }
}