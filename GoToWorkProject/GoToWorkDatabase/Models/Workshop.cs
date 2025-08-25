using System.ComponentModel.DataAnnotations.Schema;

namespace GoToWorkDatabase.Models;

internal class Workshop
{
    public required string Id { get; set; }
    public required string? ProductionId { get; set; }
    public required string Address { get; set; }
    public Production? Production { get; set; }
    [ForeignKey("WorkshopId")] public List<EmployeeWorkshop>? EmployeeWorkshops { get; set; }
}