using GoToWorkContracts.AdapterContracts;
using GoToWorkContracts.BindingModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoToWorkApi.Controllers;

[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
[Produces("application/json")]
public class WorkshopsController : ControllerBase
{
    private readonly IWorkshopAdapter _adapter;

    public WorkshopsController(IWorkshopAdapter adapter)
    {
        _adapter = adapter;
    }

    [HttpGet]
    public IActionResult GetAllRecords()
    {
        return _adapter.GetList().GetResponse(Request, Response);
    }

    [HttpGet("{productionId}")]
    public IActionResult GetListByProduction(string productionId)
    {
        return _adapter.GetListByProduction(productionId).GetResponse(Request, Response);
    }

    [HttpGet("{data}")]
    public IActionResult GetRecord(string data)
    {
        return _adapter.GetElement(data).GetResponse(Request, Response);
    }

    [HttpPost]
    public IActionResult Create([FromBody] WorkshopBindingModel model)
    {
        return _adapter.CreateWorkshop(model).GetResponse(Request, Response);
    }

    [HttpPut]
    public IActionResult Update([FromBody] WorkshopBindingModel model)
    {
        return _adapter.UpdateWorkshop(model).GetResponse(Request, Response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        return _adapter.DeleteWorkshop(id).GetResponse(Request, Response);
    }
}