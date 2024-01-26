namespace Learning.Domain.Identity;

public class ApplicationUserOtherDetail
{
    public long Id { get; set; }

    /// <summary>
    /// Users contact number.
    /// </summary>
    /// <example>+91-9770272321</example>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// First name
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Last name
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Full name of the user
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Associated user id
    /// </summary>
    public string UserId { get; set; }

    public ApplicationUser User  { get; set; }
}
