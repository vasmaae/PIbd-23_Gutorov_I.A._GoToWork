using GoToWorkContracts.Infrastructure;
using GoToWorkContracts.ViewModels;

namespace GoToWorkContracts.AdapterContracts.OperationResponses;

public class ReportOperationResponse : OperationResponse
{
    public static ReportOperationResponse OK(List<WorkshopsReportViewModel> data) =>
        OK<ReportOperationResponse, List<WorkshopsReportViewModel>>(data);

    public static ReportOperationResponse OK(List<DetailsReportViewModel> data) =>
        OK<ReportOperationResponse, List<DetailsReportViewModel>>(data);

    public static ReportOperationResponse OK(Stream data, string filename) =>
        OK<ReportOperationResponse, Stream>(data, filename);

    public static ReportOperationResponse NoContent() =>
        NoContent<ReportOperationResponse>();

    public static ReportOperationResponse NotFound(string message) =>
        NotFound<ReportOperationResponse>(message);

    public static ReportOperationResponse BadRequest(string message) =>
        BadRequest<ReportOperationResponse>(message);

    public static ReportOperationResponse InternalServerError(string message) =>
        InternalServerError<ReportOperationResponse>(message);

    public static ReportOperationResponse Unauthorized(string message) =>
        Unauthorized<ReportOperationResponse>(message);
}