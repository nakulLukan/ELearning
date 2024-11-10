using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learning.Web.Controllers;

public class AccountController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public AccountController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [Authorize]
    [HttpGet("Account/SignOut")]
    public async Task<IActionResult> LogoutUser()
    {
        // Invalidate the local session
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

        // Construct the Cognito sign-out URL
        var cognitoDomain = _configuration["Oidc:Domain"];
        var clientId = _configuration["Oidc:ClientId"];
        var signOutRedirectUri = "https://localhost:5000/";

        var cognitoSignOutUrl = $"{cognitoDomain}/logout?client_id={clientId}&logout_uri={signOutRedirectUri}";

        return Redirect(cognitoSignOutUrl);
    }
}
