namespace Learning.Shared.Extensions;

public static class DateExtensions
{
    public static string ToDateString(this DateOnly dateOnly)
    {
        return dateOnly.ToString("dd/MMM/yyyy");
    }
}
