namespace GoToWorkContracts.ViewModels;

public class DetailsReportViewModel
{
    public string DetailName { get; set; }
    public DateTime CreationDate { get; set; }
    public string Material { get; set; }
    public List<string> RelatedProductions { get; set; }
    public List<string> RelatedMachines { get; set; }
    public int QuantityInProducts { get; set; }
}