using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Learning.Web.Events;

public class ApplicationCookieAuthenticationEvents : CookieAuthenticationEvents
{
    public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
    {
        if (context.Request.Path.StartsWithSegments("/admin"))
        {
            context.RedirectUri = context.RedirectUri.Replace("/Account/Login", "/Account/Admin/Login");
        }

        return base.OnRedirectToLogin(context);
    }
}
