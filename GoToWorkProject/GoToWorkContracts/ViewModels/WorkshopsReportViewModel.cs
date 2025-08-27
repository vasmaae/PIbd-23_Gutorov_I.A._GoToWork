namespace GoToWorkContracts.ViewModels;

public class WorkshopsReportViewModel
{
    public string WorkshopId { get; set; }
    public string Address { get; set; }
    public string ProductionName { get; set; }
    public List<string> RelatedDetails { get; set; }
}