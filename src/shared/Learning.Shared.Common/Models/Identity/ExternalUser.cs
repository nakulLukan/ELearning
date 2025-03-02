namespace Learning.Shared.Common.Models.Identity;

public class ExternalUser
{
    public required string Sub { get; set; }
    public required string UserName { get; set; }
    public required DateTime CreatedOn { get; set; }
    public required DateTime LastUpdatedOn { get; set; }
    public required string? Role { get; set; }
    public required string Email { get; set; }
    public required bool IsEmailConfirmed { get; set; }
    public required string PhoneNumber { get; set; }
    public required bool IsPhoneNumberConfirmed { get; set; }
    public required string FullName { get; set; }
    public required bool IsEnabled { get; set; }
    public required bool IsAccountConfirmed { get; set; }
}
