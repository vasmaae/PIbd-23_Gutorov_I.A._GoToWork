using GoToWorkContracts.AdapterContracts.OperationResponses;
using GoToWorkContracts.BindingModels;
using GoToWorkContracts.ViewModels;

namespace GoToWorkContracts.AdapterContracts;

public interface IReportAdapter
{
    Task<ReportOperationResponse> GetWorkshopsByDetailsAsync(
        WorkshopsReportBindingModel selectedDetailIds, CancellationToken ct);
    Task<ReportOperationResponse> GetDetailsByMachinesAndProductionsAsync(
        DetailsReportBindingModel selectedDates, CancellationToken ct);
    Task<ReportOperationResponse> CreateDocxDocumentWorkshopsByDetailsAsync(
        WorkshopsReportBindingModel selectedDetailIds, CancellationToken ct);
    Task<ReportOperationResponse> CreateXlsxDocumentWorkshopsByDetailsAsync(
        WorkshopsReportBindingModel selectedDetailIds, CancellationToken ct);
    Task<ReportOperationResponse> CreatePdfDocumentDetailsByMachinesAndProductionsAsync(
        DetailsReportBindingModel selectedDates, CancellationToken ct);
    Task<ReportOperationResponse> SendPdfDocumentDetailsByMachinesAndProductionsEmailAsync(
        DetailsReportBindingModel selectedDates, CancellationToken ct);
}