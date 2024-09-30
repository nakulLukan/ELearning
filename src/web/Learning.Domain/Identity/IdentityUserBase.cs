namespace Learning.Domain.Identity;

public abstract class IdentityUserBase
{
    public long Index { get; set; }
    public DateTimeOffset AccountCreatedOn { get; set; }
    public bool IsActive { get; set; }
    public int? RoleId { get; set; }

    public AppRole? Role { get; set; }
}
