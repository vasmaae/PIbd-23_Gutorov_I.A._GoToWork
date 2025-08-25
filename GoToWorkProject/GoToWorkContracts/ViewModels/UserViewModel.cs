using GoToWorkContracts.Enums;

namespace GoToWorkContracts.ViewModels;

public class UserViewModel
{
    public required string Id { get; set; }
    public required string Login { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public UserRole Role { get; set; }
}