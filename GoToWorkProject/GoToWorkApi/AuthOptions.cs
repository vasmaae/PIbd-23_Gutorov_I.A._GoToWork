using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace GoToWorkApi;

public static class AuthOptions
{
    public const string Issuer = "GoToWorkAuthServer";
    public const string Audience = "GoToWorkAuthClient";
    private const string Key = "GoToWorkCourseWorkAuthenticationSecretKey";

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
}