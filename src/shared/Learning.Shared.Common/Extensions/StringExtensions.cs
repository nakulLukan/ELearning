namespace Learning.Shared.Common.Extensions;

public static class StringExtensions
{
    public static string ToNormalizedString(this string str)
    {
        return str.ToUpperInvariant();
    }
}
