using GoToWorkContracts.Enums;

namespace GoToWorkDatabase.Models;

public class User
{
    public required string Id { get; set; }
    public required string Login { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required UserRole Role { get; set; }
    public List<Employee>? Employees { get; set; }
}