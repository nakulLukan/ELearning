using Learning.Shared.Common.Constants;
using System.Security.Claims;
namespace Learning.Web.Client;

// Add properties to this class and update the server and client AuthenticationStateProviders
// to expose more information about the authenticated user to the client.
public sealed class UserInfo
{
    public required string UserId { get; init; }
    public required string Email { get; init; }
    public required bool IsEmailVerified { get; init; }
    public required string PhoneNumber { get; init; }
    public required bool IsPhoneNumberVerified { get; init; }
    public required string Role { get; init; }
    public required string Name { get; init; }

    public const string UserIdClaimType = ClaimConstant.Sub;
    public const string NameClaimType = ClaimConstant.Name;
    public const string RoleClaimType = ClaimConstant.AwsRoleClaim;
    public const string EmailClaimType = ClaimConstant.EmailClaim;
    public const string IsEmailVerifiedClaimType = ClaimConstant.IsEmailVerifiedClaim;
    public const string PhoneNumberClaimType = ClaimConstant.PhoneNumber;
    public const string IsPhoneNumberVerifiedClaimType = ClaimConstant.IsPhoneNumberVerifiedClaim;

    public static UserInfo FromClaimsPrincipal(ClaimsPrincipal principal) =>
        new()
        {
            UserId = GetRequiredClaim(principal, UserIdClaimType),
            Email = GetRequiredClaim(principal, EmailClaimType),
            IsEmailVerified = bool.TryParse(GetRequiredClaim(principal, IsEmailVerifiedClaimType), out bool isEmailVerifiedClaim) ? isEmailVerifiedClaim : false,
            Role = GetRequiredClaim(principal, RoleClaimType),
            Name = GetRequiredClaim(principal, NameClaimType),
            PhoneNumber = GetRequiredClaim(principal, PhoneNumberClaimType),
            IsPhoneNumberVerified = bool.TryParse(GetRequiredClaim(principal, IsPhoneNumberVerifiedClaimType), out bool isPhoneNumberVerifiedClaim) ? isPhoneNumberVerifiedClaim : false,
        };

    public ClaimsPrincipal ToClaimsPrincipal() =>
        new(new ClaimsIdentity(
            [new(UserIdClaimType, UserId), new(NameClaimType, Name), new(RoleClaimType, Role), new(EmailClaimType, Email)],
            authenticationType: nameof(UserInfo),
            nameType: NameClaimType,
            roleType: RoleClaimType));

    private static string GetRequiredClaim(ClaimsPrincipal principal, string claimType) =>
        principal.FindFirst(claimType)?.Value ?? string.Empty;
}