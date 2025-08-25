using System.Collections.Generic;

namespace GoToWorkContracts.ViewModels;

public class WorkshopViewModel
{
    public required string Id { get; set; }
    public required string? ProductionId { get; set; }
    public required string Address { get; set; }
    public required string? ProductionName { get; set; }
    public required List<EmployeeWorkshopViewModel>? Employees { get; set; }
}