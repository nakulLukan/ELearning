using Learning.Business.Contracts.HttpContext;
using Learning.Shared.Common.Constants;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Learning.Web.Client.Impl.HttpContext;

public class RequestContext : IRequestContext
{
    private readonly AuthenticationStateProvider _authStateProvider;
    private AuthenticationState _authState;

    /// <summary>
    /// Application request context.
    /// </summary>
    /// <param name="authStateProvider"></param>
    public RequestContext(AuthenticationStateProvider authStateProvider)
    {
        _authStateProvider = authStateProvider;
    }

    private async Task<bool> Init()
    {
        _authState ??= await _authStateProvider.GetAuthenticationStateAsync();

        // If not logged in then this will be null. So return it as an indicator.
        return _authState is not null;
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
            isAdmin = bool.Parse(_authState.User.Claims.First(x => x.Type == ClaimConstant.IsAdminClaim).Value);
        }

        return (isAuth, isAdmin);
    }

    public async Task<bool> IsAdmin()
    {
        if (!await Init()) return false;
        var isAdminClaim = _authState.User.Claims.FirstOrDefault(x => x.Type == ClaimConstant.IsAdminClaim)?.Value;
        return bool.Parse(isAdminClaim);
    }

    public async Task<string> GetUserId()
    {
        await Init();
        var userIdClaim = _authState.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
        return userIdClaim.Value;
    }
}
