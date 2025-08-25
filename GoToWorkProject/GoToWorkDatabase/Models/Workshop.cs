using System.ComponentModel.DataAnnotations.Schema;

namespace GoToWorkDatabase.Models;

internal class Workshop
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Address { get; set; }
    [ForeignKey("WorkshopId")] public List<EmployeeWorkshop>? EmployeeWorkshops { get; set; }
}