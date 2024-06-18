using Learning.Shared.Common.Constants;
using System.Security.Claims;
namespace Learning.Web.Client;

// Add properties to this class and update the server and client AuthenticationStateProviders
// to expose more information about the authenticated user to the client.
public sealed class UserInfo
{
    public required string UserId { get; init; }
    public required string Email { get; init; }
    public required string Role { get; init; }
    public required string Name { get; init; }
    public bool IsEmailVerified { get; init; }

    public const string UserIdClaimType = ClaimConstant.AwsUserNameClaim;
    public const string NameClaimType = ClaimConstant.AwsUserNameClaim;
    public const string RoleClaimType = ClaimConstant.AwsRoleClaim;
    public const string EmailClaimType = ClaimConstant.EmailClaim;
    public const string IsEmailVerifiedClaimType = ClaimConstant.IsEmailVerifiedClaim;

    public static UserInfo FromClaimsPrincipal(ClaimsPrincipal principal) =>
        new()
        {
            UserId = GetRequiredClaim(principal, UserIdClaimType),
            Email = GetRequiredClaim(principal, EmailClaimType),
            Role = GetRequiredClaim(principal, RoleClaimType),
            Name = GetRequiredClaim(principal, NameClaimType),
            IsEmailVerified = bool.Parse(GetRequiredClaim(principal, IsEmailVerifiedClaimType)),
        };

    public ClaimsPrincipal ToClaimsPrincipal() =>
        new(new ClaimsIdentity(
            [new(UserIdClaimType, UserId), new(NameClaimType, Name), new(RoleClaimType, Role), new(EmailClaimType, Email)],
            authenticationType: nameof(UserInfo),
            nameType: NameClaimType,
            roleType: RoleClaimType));

    private static string GetRequiredClaim(ClaimsPrincipal principal, string claimType) =>
        principal.FindFirst(claimType)?.Value ?? throw new InvalidOperationException($"Could not find required '{claimType}' claim.");
}