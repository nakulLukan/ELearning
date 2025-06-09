using System.Globalization;
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

    public static string ToHumanName(this string str)
    {
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

        // Convert the input string to lowercase
        string lowerCaseInput = str.ToLower();

        // Use TextInfo.ToTitleCase to convert to Title Case
        return textInfo.ToTitleCase(lowerCaseInput);
    }

    public static string TrimToLen(this string str, int length)
    {
        if (str.Length > length)
        {
            return str.Substring(0, length);
        }

        return str;
    }
}
