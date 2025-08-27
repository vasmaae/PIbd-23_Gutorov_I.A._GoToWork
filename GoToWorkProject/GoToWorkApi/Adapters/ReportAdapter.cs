using GoToWorkContracts.AdapterContracts;
using GoToWorkContracts.AdapterContracts.OperationResponses;
using GoToWorkContracts.BindingModels;
using GoToWorkContracts.BusinessLogicContracts;
using GoToWorkContracts.ViewModels;

namespace GoToWorkApi.Adapters;

public class ReportAdapter : IReportAdapter
{
    private readonly IReportContract _reportBusinessLogic;
    private readonly ILogger _logger;

    public ReportAdapter(IReportContract reportBusinessLogic, ILogger<ReportAdapter> logger)
    {
        _reportBusinessLogic = reportBusinessLogic;
        _logger = logger;
    }

    public async Task<ReportOperationResponse> GetWorkshopsByDetailsAsync(WorkshopsReportBindingModel selectedDetailIds,
        CancellationToken ct)
    {
        try
        {
            var data = await _reportBusinessLogic.GetWorkshopsByDetailsAsync(selectedDetailIds.DetailIds, ct);
            return ReportOperationResponse.OK(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting workshops by details report");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public async Task<ReportOperationResponse> GetDetailsByMachinesAndProductionsAsync(
        DetailsReportBindingModel selectedDates, CancellationToken ct)
    {
        try
        {
            var data = await _reportBusinessLogic.GetDetailsByMachinesAndProductionsAsync(selectedDates.startDate,
                selectedDates.endDate, ct);
            return ReportOperationResponse.OK(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting details by machines and productions report");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public async Task<ReportOperationResponse> CreateDocxDocumentWorkshopsByDetailsAsync(
        WorkshopsReportBindingModel selectedDetailIds, CancellationToken ct)
    {
        try
        {
            var stream =
                await _reportBusinessLogic.CreateDocxDocumentWorkshopsByDetailsAsync(
                    await _reportBusinessLogic.GetWorkshopsByDetailsAsync(
                        selectedDetailIds.DetailIds, ct), ct);
            return ReportOperationResponse.OK(stream, "WorkshopsReportViewModel.docx");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating DOCX document");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public async Task<ReportOperationResponse> CreateXlsxDocumentWorkshopsByDetailsAsync(
        WorkshopsReportBindingModel selectedDetailIds, CancellationToken ct)
    {
        try
        {
            var stream =
                await _reportBusinessLogic.CreateXlsxDocumentWorkshopsByDetailsAsync(
                    await _reportBusinessLogic.GetWorkshopsByDetailsAsync(
                        selectedDetailIds.DetailIds, ct), ct);
            return ReportOperationResponse.OK(stream, "WorkshopsReportViewModel.xlsx");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating XLSX document");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public async Task<ReportOperationResponse> CreatePdfDocumentDetailsByMachinesAndProductionsAsync(
        DetailsReportBindingModel selectedDates, CancellationToken ct)
    {
        try
        {
            var stream =
                await _reportBusinessLogic.CreatePdfDocumentDetailsByMachinesAndProductionsAsync(
                    await _reportBusinessLogic.GetDetailsByMachinesAndProductionsAsync(
                        selectedDates.startDate, selectedDates.endDate, ct),
                    selectedDates.startDate, selectedDates.endDate, ct);
            return ReportOperationResponse.OK(stream, "DetailsReport.pdf");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating PDF document");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public async Task<ReportOperationResponse> SendPdfDocumentDetailsByMachinesAndProductionsEmailAsync(
        DetailsReportBindingModel selectedDates, CancellationToken ct)
    {
        try
        {
            var report = await _reportBusinessLogic.CreatePdfDocumentDetailsByMachinesAndProductionsAsync(
                await _reportBusinessLogic.GetDetailsByMachinesAndProductionsAsync(
                    selectedDates.startDate, selectedDates.endDate, ct),
                selectedDates.startDate, selectedDates.endDate, ct);
            await _reportBusinessLogic.SendEmailAsync(report, selectedDates.email!, "Отчёт о деталях",
                "DetailsReport.pdf", "application/pdf");
            return ReportOperationResponse.NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }
}