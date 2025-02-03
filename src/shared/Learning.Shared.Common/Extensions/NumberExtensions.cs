using Humanizer;
using System.Globalization;

namespace Learning.Shared.Common.Extensions;

public static class NumberExtensions
{
    private static readonly CultureInfo _cultureInfo = new CultureInfo("en-IN");
    public static string ToFileSizeString(this long size)
    {
        return size.Bytes().ToString();
    }

    public static string ToDurationString(this int numOfSeconds)
    {
        return numOfSeconds.Seconds().Humanize(2);
    }

    public static string ToCurrency(this float? value)
    {
        if (value == null)
        {
            return string.Empty;
        }

        return value.Value.ToString("C", _cultureInfo);
    }

    public static string ToCurrency(this float value)
    {
        return value.ToString("C", _cultureInfo);
    }
}
