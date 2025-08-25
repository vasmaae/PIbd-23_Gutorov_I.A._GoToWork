using GoToWorkContracts.AdapterContracts;
using GoToWorkContracts.BindingModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoToWorkApi.Controllers;

[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
[Produces("application/json")]
public class EmployeesController(IEmployeeAdapter adapter) : ControllerBase
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
    public IActionResult Create([FromBody] EmployeeBindingModel model)
    {
        return adapter.CreateEmployee(model).GetResponse(Request, Response);
    }

    [HttpPut]
    public IActionResult Update([FromBody] EmployeeBindingModel model)
    {
        return adapter.UpdateEmployee(model).GetResponse(Request, Response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        return adapter.DeleteEmployee(id).GetResponse(Request, Response);
    }
}