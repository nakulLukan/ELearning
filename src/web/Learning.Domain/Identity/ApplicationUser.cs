using Microsoft.AspNetCore.Identity;

namespace Learning.Domain.Identity;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public bool IsAdmin { get; set; }
    public long Index { get; set; }
    public DateTimeOffset AccountCreatedOn { get; set; }
    public bool IsActive { get; set; }

    public ApplicationUserOtherDetail OtherDetails { get; set; }
}
