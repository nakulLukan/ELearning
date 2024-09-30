
namespace Learning.Domain.Identity;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUserBase
{
    public required string Id { get; set; }
    public bool IsAdmin { get; set; }

    public ApplicationUserOtherDetail OtherDetails { get; set; } = new();
}
