namespace Learning.Business.Dto.Identity;

public class VerifyOtpResponseDto
{
    public bool Matched { get; set; }

    /// <summary>
    /// Complete phone number
    /// </summary>
    public string? Username { get; set; }
}
