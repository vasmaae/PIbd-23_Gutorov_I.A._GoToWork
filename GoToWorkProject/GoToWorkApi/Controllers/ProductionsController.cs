using GoToWorkContracts.AdapterContracts;
using GoToWorkContracts.BindingModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoToWorkApi.Controllers;

[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
[Produces("application/json")]
public class ProductionsController : ControllerBase
{
    private readonly IProductionAdapter _adapter;

    public ProductionsController(IProductionAdapter adapter)
    {
        _adapter = adapter;
    }

    [HttpGet]
    public IActionResult GetAllRecords()
    {
        return _adapter.GetList().GetResponse(Request, Response);
    }

    [HttpGet("{data}")]
    public IActionResult GetRecord(string data)
    {
        return _adapter.GetElement(data).GetResponse(Request, Response);
    }

    [HttpPost]
    public IActionResult Create([FromBody] ProductionBindingModel model)
    {
        return _adapter.CreateProduction(model).GetResponse(Request, Response);
    }

    [HttpPut]
    public IActionResult Update([FromBody] ProductionBindingModel model)
    {
        return _adapter.UpdateProduction(model).GetResponse(Request, Response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        return _adapter.DeleteProduction(id).GetResponse(Request, Response);
    }
}