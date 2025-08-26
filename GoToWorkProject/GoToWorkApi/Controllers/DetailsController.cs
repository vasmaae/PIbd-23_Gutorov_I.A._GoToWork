using GoToWorkContracts.AdapterContracts;
using GoToWorkContracts.BindingModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoToWorkApi.Controllers;

[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
[Produces("application/json")]
public class DetailsController(IDetailAdapter adapter) : ControllerBase
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

    [HttpPost]
    public IActionResult Register([FromBody] DetailBindingModel model)
    {
        return adapter.RegisterDetail(model).GetResponse(Request, Response);
    }

    [HttpPut]
    public IActionResult ChangeInfo([FromBody] DetailBindingModel model)
    {
        return adapter.ChangeDetailInfo(model).GetResponse(Request, Response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        return adapter.RemoveDetail(id).GetResponse(Request, Response);
    }
}