using Humanizer;
using System.Diagnostics.CodeAnalysis;

namespace Learning.Shared.Common.Extensions;

public static class DateExtensions
{
    private static readonly TimeZoneInfo _timeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

    public static string ToDateString(this DateTimeOffset? dateTime)
    {
        return dateTime.HasValue ? dateTime.Value.ToString("yyyy-MM-dd") : string.Empty;
    }

    public static string ToFileTimeString(this DateTimeOffset dateTime)
    {
        return dateTime.ToString("yyyyMMddHHmmss");
    }

    /// <summary>
    /// Returns date time string "yyyy-MM-dd HH:mm:ss"
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string ToLocalDateTimeString(this DateTimeOffset dateTime)
    {
        return dateTime.ToOffset(_timeZone.BaseUtcOffset).ToString("yyyy-MM-dd HH:mm:ss");
    }

    /// <summary>
    /// Returns humanized form of date time value (2 hours ago, 1 day ago...)
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string ToHumanizedDateTimeString(this DateTimeOffset dateTime)
    {
        return dateTime.Humanize();
    }

    /// <summary>
    /// Returns date time string "yyyy-MM-dd HH:mm:ss"
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string ToLocalDateTimeString(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }

    /// <summary>
    /// Returns date time string "yyyy-MM-dd"
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string ToLocalDateString(this DateTimeOffset dateTime)
    {
        return dateTime.ToOffset(_timeZone.BaseUtcOffset).ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// Returns date string with specified format
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string ToLocalDateFormatedString(this DateTimeOffset dateTime, [StringSyntax(StringSyntaxAttribute.DateTimeFormat)] string format = "dd MMM yyyy")
    {
        return dateTime.ToOffset(_timeZone.BaseUtcOffset).ToString(format);
    }
    
    /// <summary>
    /// Returns date string with specified format
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string ToLocalDateFormatedString(this DateOnly dateTime, [StringSyntax(StringSyntaxAttribute.DateTimeFormat)] string format = "dd MMM yyyy")
    {
        return dateTime.ToString(format);
    }

    /// <summary>
    /// Converted given number of minutes into string in following format <b>HH hours, MM minutes, SS seconds</b>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToHoursMinutesSeconds(this TimeSpan? value)
    {
        if (value.HasValue)
        {
            return value.Value
                .Humanize(3,
                maxUnit: Humanizer.Localisation.TimeUnit.Hour,
                minUnit: Humanizer.Localisation.TimeUnit.Second);
        }

        return String.Empty;
    }

    public static string ToTimeString(this TimeSpan timeSpan)
    {
        return DateTime.Today.Add(timeSpan).ToString("hh\\:mm tt");
    }
}
