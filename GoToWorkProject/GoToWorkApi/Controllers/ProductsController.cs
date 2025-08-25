using GoToWorkContracts.AdapterContracts;
using GoToWorkContracts.BindingModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoToWorkApi.Controllers;

[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly IProductAdapter _adapter;

    public ProductsController(IProductAdapter adapter)
    {
        _adapter = adapter;
    }

    [HttpGet]
    public IActionResult GetAllRecords()
    {
        return _adapter.GetList().GetResponse(Request, Response);
    }

    [HttpGet("{machineId}")]
    public IActionResult GetListByMachine(string machineId)
    {
        return _adapter.GetListByMachine(machineId).GetResponse(Request, Response);
    }

    [HttpGet]
    public IActionResult GetListByCreationDate(DateTime from, DateTime to)
    {
        return _adapter.GetListByCreationDate(from, to).GetResponse(Request, Response);
    }

    [HttpGet("{data}")]
    public IActionResult GetRecord(string data)
    {
        return _adapter.GetElement(data).GetResponse(Request, Response);
    }

    [HttpPost]
    public IActionResult Create([FromBody] ProductBindingModel model)
    {
        return _adapter.CreateProduct(model).GetResponse(Request, Response);
    }

    [HttpPut]
    public IActionResult Update([FromBody] ProductBindingModel model)
    {
        return _adapter.UpdateProduct(model).GetResponse(Request, Response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        return _adapter.DeleteProduct(id).GetResponse(Request, Response);
    }
}