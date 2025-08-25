using GoToWorkContracts.AdapterContracts;
using GoToWorkContracts.BindingModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoToWorkApi.Controllers;

[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
[Produces("application/json")]
public class MachinesController : ControllerBase
{
    private readonly IMachineAdapter _adapter;

    public MachinesController(IMachineAdapter adapter)
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
    public IActionResult Create([FromBody] MachineBindingModel model)
    {
        return _adapter.CreateMachine(model).GetResponse(Request, Response);
    }

    [HttpPut]
    public IActionResult Update([FromBody] MachineBindingModel model)
    {
        return _adapter.UpdateMachine(model).GetResponse(Request, Response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        return _adapter.DeleteMachine(id).GetResponse(Request, Response);
    }
}