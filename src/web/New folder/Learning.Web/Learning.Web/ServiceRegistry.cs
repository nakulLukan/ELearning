using Learning.Business;
using Learning.Business.Contracts.HttpContext;
using Learning.Infrastructure;
using Learning.Web.Client.Constants;
using Learning.Web.Client.Contracts.Events;
using Learning.Web.Client.Contracts.Presentation;
using Learning.Web.Client.Impl.Events;
using Learning.Web.Client.Impl.HttpContext;
using Learning.Web.Client.Impl.Presentation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using MudBlazor.Services;
using System.Globalization;
namespace Learning.Web;

public static class ServiceRegistry
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.RegisterWebServices(builder.Configuration);
        builder.Services.RegisterInfrastructureServices(builder.Configuration);
        builder.Services.RegisterBusinessServices(builder.Configuration);
    }

    private static void RegisterWebServices(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        #region AddConfiruation
        builder.Configuration.AddConfiguration(new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>()
                .Build());
        #endregion
        builder.Services.AddMudServices();
        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        builder.Services.AddCascadingAuthenticationState();

        #region Add OpenIDConnect
        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(options =>
            {
                options.Authority = configuration["Oidc:Authority"];
                options.ClientId = configuration["Oidc:ClientId"];
                options.ClientSecret = configuration["Oidc:ClientSecret"];
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.SaveTokens = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                };

                options.Scope.Clear();
                foreach (var scope in configuration["Oidc:Scope"].Split(' '))
                {
                    options.Scope.Add(scope);
                }
            });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyConstant.AdminPolicy, policy =>
            {
                policy.RequireClaim("cognito:groups", "admin");
            });
        });
        #endregion

        builder.Services.AddMediatR((c) =>
        {
            c.RegisterServicesFromAssembly(typeof(Business.ServiceRegistry).Assembly);
        });

        #region Localization
        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en-IN")
            };

            options.DefaultRequestCulture = new RequestCulture(supportedCultures[0]);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });
        #endregion

        RegisterAppServices(builder);
    }

    private static void RegisterAppServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IToastService, ToastService>();
        builder.Services.AddScoped<IAppMediator, AppMediator>();
        builder.Services.AddScoped<IRequestContext, RequestContext>();
    }
}
