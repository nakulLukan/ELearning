using Learning.Shared.Common.Constants;
using Microsoft.AspNetCore.Components.Authorization;

namespace Learning.Web.Client.Impl.HttpContext;

public abstract class RequestContextBase
{
    protected readonly AuthenticationStateProvider AuthStateProvider;

    public RequestContextBase(AuthenticationStateProvider authStateProvider)
    {
        AuthStateProvider = authStateProvider;
    }

    private AuthenticationState _authState;

    private async Task Init()
    {
        _authState ??= await AuthStateProvider.GetAuthenticationStateAsync();
    }

    public async Task<bool> IsAuthenticated()
    {
        await Init();
        return _authState.User.Identity.IsAuthenticated;
    }

    public async Task<(bool IsAuthenticated, bool IsAdmin)> IsLoggedIn()
    {
        if (!await IsAuthenticated())
        {
            return (false, false);
        }
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
        if (!await IsAuthenticated()) return string.Empty;
        var userIdClaim = _authState.User.Claims.First(x => x.Type == ClaimConstant.AwsUserNameClaim);
        return userIdClaim.Value;
    }

    public async Task<string> GetUserRole()
    {
        if (!await IsAuthenticated()) return string.Empty;
        var userRole = _authState.User.Claims.First(x => x.Type == ClaimConstant.AwsRoleClaim);
        return userRole.Value;
    }

    public async Task<string> GetName()
    {
        if (!await IsAuthenticated()) return string.Empty;
        var name = _authState.User.Claims.First(x => x.Type == ClaimConstant.Name);
        return name.Value;
    }

    public async Task<string?> GetEmail()
    {
        if (!await IsAuthenticated()) return string.Empty;
        var email = _authState.User.Claims.FirstOrDefault(x => x.Type == ClaimConstant.EmailClaim);
        return email?.Value;
    }

    public async Task<string> GetPhoneNumber()
    {
        if (!await IsAuthenticated()) return string.Empty;
        var phoneNumber = _authState.User.Claims.First(x => x.Type == ClaimConstant.PhoneNumber);
        return phoneNumber.Value;
    }
}
