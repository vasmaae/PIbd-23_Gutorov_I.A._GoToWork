using System.ComponentModel.DataAnnotations.Schema;

namespace GoToWorkDatabase.Models;

internal class Employee
{
    public required string Id { get; set; }
    public required string FullName { get; set; }
    [ForeignKey("EmployeeId")] public List<EmployeeMachine>? EmployeeMachines { get; set; }
    [ForeignKey("EmployeeId")] public List<EmployeeWorkshop>? EmployeeWorkshops { get; set; }
}