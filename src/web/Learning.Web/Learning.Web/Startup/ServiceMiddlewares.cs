using System.Globalization;
using Learning.Shared.Constants;
using Learning.Web.Utilities.ExceptionHandler;
namespace Learning.Web;

public static class ServiceMiddlewares
{
    public static void RegisterMiddlewares(this WebApplication app)
    {
        #region Localization
        var supportedCultures = new[]
        {
            new CultureInfo("en-IN"),
        };
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-IN"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });
        #endregion
        app.UseMiddleware<ApiExceptionHandlerMiddleware>();

        #region Add ClientId
        app.Use(async (context, next) =>
        {
            if (!context.Request.Cookies.ContainsKey(CookieConstant.ClientId))
            {
                var clientId = Guid.NewGuid().ToString();
                context.Response.Cookies.Append(CookieConstant.ClientId, clientId, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTimeOffset.MaxValue
                });
            }
            await next();
        });
        #endregion
    }
}
