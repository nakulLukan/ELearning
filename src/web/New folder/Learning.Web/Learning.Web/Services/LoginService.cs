using Learning.Domain.Identity;
using Learning.Shared.Common.Constants;
using System.Security.Claims;

namespace Learning.Web.Services;

public class LoginService
{
    public static async Task<List<Claim>> AddClaims(ApplicationUser user)
    {
        List<Claim> additionalClaims = new();
        additionalClaims.Add(new Claim(ClaimConstant.IsAdminClaim, user.IsAdmin.ToString()));
        return additionalClaims;
    }
}
