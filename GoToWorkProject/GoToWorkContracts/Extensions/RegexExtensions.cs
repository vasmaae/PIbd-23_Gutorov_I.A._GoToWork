using System.Text.RegularExpressions;

namespace GoToWorkContracts.Extensions;

public static partial class RegexExtensions
{
    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
    public static partial Regex EmailRegex();
    
    [GeneratedRegex(@"^(?=.*[A-Z])(?=.*\W).{8,}$")]
    public static partial Regex PasswordRegex();
}