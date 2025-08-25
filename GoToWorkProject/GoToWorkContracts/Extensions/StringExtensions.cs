namespace GoToWorkContracts.Extensions;

public static class StringExtensions
{
    public static bool IsEmpty(this string str) => string.IsNullOrWhiteSpace(str);

    public static bool IsGuid(this string str) => Guid.TryParse(str, out _);
}