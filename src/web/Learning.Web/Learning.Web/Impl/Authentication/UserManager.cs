using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Learning.Business.Requests.Identity;
using Learning.Shared.Application.Contracts.Identity;
using Learning.Shared.Application.Exceptions.Identity;
using Learning.Web.Contracts.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Learning.Web.Impl.Authentication;

public class UserManager : IUserManager
{
    private readonly IExternalIdentityProvider _identityProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMediator _mediator;

    public UserManager(IExternalIdentityProvider identityProvider, IHttpContextAccessor httpContextAccessor, IMediator mediator)
    {
        _identityProvider = identityProvider;
        _httpContextAccessor = httpContextAccessor;
        _mediator = mediator;
    }

    public async Task Login(string username, string password)
    {
        try
        {
            var signinResponse = await _identityProvider.Login(username, password);

            var claims = GetClaimsFromToken(signinResponse.IdToken);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
            };
            authProperties.StoreTokens([
                new() { Name = "access_token", Value = signinResponse.AccessToken },
                    new() { Name = "refresh_token", Value =  signinResponse.RefreshToken },
                    new() { Name = "expires_at", Value = signinResponse.ExpiresIn.ToString("o", CultureInfo.InvariantCulture) },
                ]);
            await _httpContextAccessor!.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);
        }
        catch (ExternalIdentityProviderException ex)
        {
            if (ex.Type == ExternalIdentityProviderExceptionType.AccountNotConfirmed)
            {
                _ = await SendOtpForAccountConfirmation(username);
            }
            throw;
        }
    }

    public async Task<long> SignUpUser(string username, string password, string name, string address)
    {
        // Create user in identity service.
        // If account exists then check if the phone number is verified. If not then send otp to confirm account.
        try
        {
            await _identityProvider.SignUpUser(username, password, name, address);
            return await SendOtpForAccountConfirmation(username);
        }
        catch (ExternalIdentityProviderException ex)
        {
            if (ex.Type == ExternalIdentityProviderExceptionType.UserAlreadyExists)
            {
                var details = await _identityProvider.GetUserDetailsByUsername(username);
                if (!details.IsAccountConfirmed && !details.IsPhoneNumberConfirmed)
                {
                    return await SendOtpForAccountConfirmation(username);
                }
            }

            throw;
        }
    }

    private async Task<long> SendOtpForAccountConfirmation(string username)
    {
        return (await _mediator.Send(new CreateOtpCommand() { MobileNumber = username })).Data;
    }

    public async Task<bool> ConfirmAccount(string username, int otp)
    {
        try
        {
            var response = await _mediator.Send(new VerifyOtpCommand { Otp = otp, MobileNumber = username });
            if (response.Matched)
            {
                await _identityProvider.ConfirmUser(username!);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    private static IEnumerable<Claim> GetClaimsFromToken(string idToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(idToken);

        return token.Claims;
    }

    public async Task Logout()
    {
        await _httpContextAccessor!.HttpContext!.SignOutAsync();
        var authResult = await _httpContextAccessor.HttpContext!.AuthenticateAsync();
        if (authResult.Succeeded)
        {
            var accessToken = authResult.Ticket?.Properties.GetTokenValue("access_token");
            if (!string.IsNullOrEmpty(accessToken))
            {
                await _identityProvider.SignOut(accessToken);
            }
        }
    }
}
