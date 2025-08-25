using System.ComponentModel.DataAnnotations.Schema;

namespace GoToWorkDatabase.Models;

internal class Employee
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string UserId { get; set; }
    public required string FullName { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DateOfDelete { get; set; }
    public User? User { get; set; }
    [ForeignKey("EmployeeId")] public List<Machine>? Machines { get; set; }
    [ForeignKey("EmployeeId")] public List<Workshop>? Workshops { get; set; }
}