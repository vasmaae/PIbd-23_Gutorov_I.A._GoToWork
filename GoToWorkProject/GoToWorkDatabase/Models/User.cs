using System.ComponentModel.DataAnnotations.Schema;

namespace GoToWorkDatabase.Models;

internal class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Login { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public bool IsDeleted { get; set; }
    [ForeignKey("UserId")] public List<Detail>? Details { get; set; }
    [ForeignKey("UserId")] public List<Employee>? Employees { get; set; }
}