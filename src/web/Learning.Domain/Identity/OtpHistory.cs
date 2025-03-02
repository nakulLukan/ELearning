namespace Learning.Domain.Identity;

public class OtpHistory
{
    public long Id { get; set; }
    public string? UserName { get; set; }
    public int Otp { get; set; }
    public bool IsUsed { get; set; }
}
