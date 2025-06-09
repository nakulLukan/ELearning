namespace Learning.Domain.Identity;

public class ApplicationUserOtherDetail
{
    public long Id { get; set; }

    /// <summary>
    /// Users contact number.
    /// </summary>
    /// <example>+91-9770272321</example>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// First name
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// Associated user id
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Date of birth of the user
    /// </summary>
    public int? YearOfBirth { get; set; }

    public string? Email { get; set; }
    public string? NormalizedEmail { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public required string Place { get; set; }
}
