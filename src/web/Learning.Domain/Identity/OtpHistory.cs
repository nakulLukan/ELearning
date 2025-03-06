namespace Learning.Domain.Identity;

public class OtpHistory
{
    public long Id { get; set; }
    public string? UserName { get; set; }
    public int Otp { get; set; }
    public DateTimeOffset ExpiresOn { get; set; }

    public DateTimeOffset? NextOtpAfter { get; set; }
    public int TimesRequested { get; set; }
}
