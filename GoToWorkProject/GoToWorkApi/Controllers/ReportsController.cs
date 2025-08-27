using GoToWorkContracts.AdapterContracts;
using GoToWorkContracts.BindingModels;
using GoToWorkContracts.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoToWorkApi.Controllers;

[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
public class ReportsController(IReportAdapter adapter) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> GetWorkshopsReportXlsx([FromBody] WorkshopsReportBindingModel workshopsReport,
        CancellationToken ct)
    {
        return (await adapter.CreateXlsxDocumentWorkshopsByDetailsAsync(workshopsReport, ct))
            .GetResponse(Request, Response);
    }

    [HttpPost]
    public async Task<IActionResult> GetWorkshopsReportDocx([FromBody] WorkshopsReportBindingModel workshopsReport,
        CancellationToken ct)
    {
        return (await adapter.CreateDocxDocumentWorkshopsByDetailsAsync(workshopsReport, ct))
            .GetResponse(Request, Response);
    }

    [HttpPost]
    public async Task<IActionResult> GetDetailsReportPdf([FromBody] DetailsReportBindingModel detailsReport,
        CancellationToken ct)
    {
        return (await adapter.CreatePdfDocumentDetailsByMachinesAndProductionsAsync(detailsReport, ct))
            .GetResponse(Request, Response);
    }

    [HttpPost]
    public async Task<IActionResult> GetDetailsReportPdfEmail([FromBody] DetailsReportBindingModel model,
        CancellationToken ct)
    {
        return (await adapter.SendPdfDocumentDetailsByMachinesAndProductionsEmailAsync(model, ct))
            .GetResponse(Request, Response);
    }
}