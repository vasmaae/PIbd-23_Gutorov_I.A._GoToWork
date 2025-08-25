namespace GoToWorkDatabase.Models;

internal class EmployeeWorkshop
{
    public required string EmployeeId { get; set; }
    public required string WorkshopId { get; set; }
    public Employee? Employee { get; set; }
    public Workshop? Workshop { get; set; }
}