using GoToWorkContracts.Enums;

namespace GoToWorkContracts.BindingModels;

public class UserRegisterBindingModel
{
    public string? Login { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public UserRole Role { get; set; }
}