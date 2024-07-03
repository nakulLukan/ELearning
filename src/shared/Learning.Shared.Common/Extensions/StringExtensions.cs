using System.Text.RegularExpressions;

namespace Learning.Shared.Common.Extensions;

public static class StringExtensions
{
    public static string ToNormalizedString(this string str)
    {
        return str.ToUpperInvariant();
    }

    public static string RemoveEmptyLines(this string str)
    {
        return Regex.Replace(str, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
    }
}
