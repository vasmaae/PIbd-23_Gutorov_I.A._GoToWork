using System.Text.RegularExpressions;
using GoToWorkContracts.Enums;
using GoToWorkContracts.Exceptions;
using GoToWorkContracts.Extensions;
using GoToWorkContracts.Infrastructure;

namespace GoToWorkContracts.DataModels;

public class UserDataModel(
    string id,
    string login,
    string email,
    string password,
    UserRole role) : IValidation
{
    public string Id { get; } = id;
    public string Login { get; } = login;
    public string Email { get; } = email;
    public string Password { get; } = password;
    public UserRole Role { get; } = role;

    public void Validate()
    {
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");
        if (!Id.IsGuid())
            throw new ValidationException("The value in the field Id is not a Guid");
        if (Login.IsEmpty())
            throw new ValidationException("Field Login is empty");
        if (Email.IsEmpty())
            throw new ValidationException("Field Email is empty");
        if (!RegexExtensions.EmailRegex().IsMatch(Email))
            throw new ValidationException("Field Email is not a valid email address");
        if (Password.IsEmpty())
            throw new ValidationException("Field Password is empty");
        if (!RegexExtensions.PasswordRegex().IsMatch(Password))
            throw new ValidationException("Field Password is not a valid password");
        if (Role == UserRole.None)
            throw new ValidationException("Field Role is empty");
    }
}