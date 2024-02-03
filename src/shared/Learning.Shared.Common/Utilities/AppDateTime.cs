namespace Learning.Shared.Common.Utilities;

public class AppDateTime
{
    public static readonly TimeZoneInfo TimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
    public static DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    public static DateTimeOffset LocalNow => DateTimeOffset.UtcNow.ToOffset(TimeZone.BaseUtcOffset);
    public static DateTimeOffset UtcNowAtStartOfTheDay => DateTimeOffset.UtcNow.Date.ToUniversalTime();
    public static DateTimeOffset UtcDateBeforeNDays(int numberOfDays) => UtcNowAtStartOfTheDay.AddDays(-numberOfDays).ToUniversalTime();
    public static DateTimeOffset TillEndOfDay => UtcNowAtStartOfTheDay.AddDays(1);
}
