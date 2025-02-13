using System.Globalization;
using Learning.Web.Utilities.ExceptionHandler;
namespace Learning.Web;

public static class ServiceMiddlewares
{
    public static void RegisterMiddlewares(this WebApplication app)
    {
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
        app.UseMiddleware<ApiExceptionHandlerMiddleware>();
    }
}
