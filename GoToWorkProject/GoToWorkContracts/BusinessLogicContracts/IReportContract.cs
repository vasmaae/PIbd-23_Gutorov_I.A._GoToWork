using GoToWorkContracts.DataModels;
using GoToWorkContracts.ViewModels;

namespace GoToWorkContracts.BusinessLogicContracts;

public interface IReportContract
{
    Task<List<WorkshopsReportViewModel>> GetWorkshopsByDetailsAsync(List<string> selectedDetailIds,
        CancellationToken ct);
    Task<Stream> CreateDocxDocumentWorkshopsByDetailsAsync(List<WorkshopsReportViewModel> data, CancellationToken ct);
    Task<Stream> CreateXlsxDocumentWorkshopsByDetailsAsync(List<WorkshopsReportViewModel> data, CancellationToken ct);

    Task<List<DetailsReportViewModel>> GetDetailsByMachinesAndProductionsAsync(
        DateTime startDate, DateTime endDate, CancellationToken ct);
    Task<Stream> CreatePdfDocumentDetailsByMachinesAndProductionsAsync(List<DetailsReportViewModel> data,
        DateTime startDate, DateTime endDate, CancellationToken ct);
    
    public Task SendEmailAsync(Stream fileStream, string recipientEmail, string subject, string fileName,
        string contentType);
}