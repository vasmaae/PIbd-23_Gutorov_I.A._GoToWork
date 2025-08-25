namespace GoToWorkContracts.BindingModels;

public class WorkshopBindingModel
{
    public string? Id { get; set; }
    public string? ProductionId { get; set; }
    public string? Address { get; set; }
    public List<EmployeeWorkshopBindingModel>? Employees { get; set; }
}