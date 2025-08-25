using GoToWorkContracts.AdapterContracts.OperationResponses;
using GoToWorkContracts.BindingModels;

namespace GoToWorkContracts.AdapterContracts;

public interface IReportAdapter
{
    ReportOperationResponse GetProductsReport(ReportBindingModel model);
    ReportOperationResponse GetDetailsReport(ReportBindingModel model);
}