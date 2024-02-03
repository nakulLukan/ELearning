using Humanizer;

namespace Learning.Shared.Common.Extensions;

public static class NumberExtensions
{
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
        if(value == null)
        {
            return string.Empty;
        }

        return value.Value.ToString("C", System.Globalization.CultureInfo.CurrentCulture);
    }

    public static string ToCurrency(this float value)
    {
        return value.ToString("C", System.Globalization.CultureInfo.CurrentCulture);
    }
}
