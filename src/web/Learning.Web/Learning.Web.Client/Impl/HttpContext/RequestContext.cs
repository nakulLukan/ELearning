using Learning.Shared.Common.Constants;
using Learning.Shared.Contracts.HttpContext;
using Microsoft.AspNetCore.Components.Authorization;

namespace Learning.Web.Client.Impl.HttpContext;

public class RequestContext : IRequestContext
{
    private readonly AuthenticationStateProvider _authStateProvider;

    public RequestContext(AuthenticationStateProvider authStateProvider)
    {
        _authStateProvider = authStateProvider;
    }

    private AuthenticationState _authState;

    private async Task Init()
    {
        _authState ??= await _authStateProvider.GetAuthenticationStateAsync();
    }

    public async Task<bool> IsAuthenticated()
    {
        await Init();
        return _authState.User.Identity.IsAuthenticated;
    }

    public async Task<(bool IsAuthenticated, bool IsAdmin)> IsLoggedIn()
    {
        await Init();
        var isAuth = _authState.User.Identity.IsAuthenticated;
        var isAdmin = false;
        if (isAuth)
        {
            isAdmin = await IsAdmin();
        }

        return (isAuth, isAdmin);
    }

    public async Task<bool> IsAdmin()
    {
        if (!await IsAuthenticated()) return false;
        var isAdminClaim = !string.IsNullOrEmpty(_authState.User.Claims.FirstOrDefault(x => x.Type == ClaimConstant.AwsRoleClaim)?.Value);
        return isAdminClaim;
    }

    public async Task<string> GetUserId()
    {
        await Init();
        var userIdClaim = _authState.User.Claims.First(x => x.Type == ClaimConstant.AwsUserNameClaim);
        return userIdClaim.Value;
    }

    public async Task<string> GetUserRole()
    {
        await Init();
        var userRole = _authState.User.Claims.First(x => x.Type == ClaimConstant.AwsRoleClaim);
        return userRole.Value;
    }
}
