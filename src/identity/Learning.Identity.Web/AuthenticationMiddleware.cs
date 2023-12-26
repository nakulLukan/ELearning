using Learning.Identity.Web.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Web;

namespace Learning.Identity.Web;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    public static IDictionary<Guid, LoginInfo> Logins { get; private set; } = new Dictionary<Guid, LoginInfo>();

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, SignInManager<ApplicationUser> signInMgr)
    {
        if (context.Request.Path == "/login" && context.Request.Query.ContainsKey("Key"))
        {
            var key = Guid.Parse(context.Request.Query["key"]);
            var info = Logins[key];

            var result = await signInMgr.PasswordSignInAsync(info.UserName, info.Password, false, lockoutOnFailure: true);
            info.Password = null;
            if (result.Succeeded)
            {
                Logins.Remove(key);
                var redirectUrl = HttpUtility.UrlDecode(context.Request.Query["ReturnUrl"]);
                context.Response.Redirect(redirectUrl);
                return;
            }
            else if (result.RequiresTwoFactor)
            {
                //TODO: redirect to 2FA razor component
                context.Response.Redirect("/loginwith2fa/" + key);
                return;
            }
            else
            {
                //TODO: Proper error handling
                context.Response.Redirect("/loginfailed");
                return;
            }
        }
        else if (context.Request.Path == "/sign-out")
        {
            await signInMgr.SignOutAsync();
            context.Response.Redirect("/login");
        }
        else
        {
            await _next.Invoke(context);
        }
    }
}

public class LoginInfo
{
    public string UserName { get; set; }
    public string Password { get; set; }
}
