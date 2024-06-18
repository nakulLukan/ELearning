using Learning.Business;
using Learning.Business.Contracts.HttpContext;
using Learning.Infrastructure;
using Learning.Shared.Common.Constants;
using Learning.Web.Client.Constants;
using Learning.Web.Client.Contracts.Events;
using Learning.Web.Client.Contracts.Presentation;
using Learning.Web.Client.Impl.Events;
using Learning.Web.Client.Impl.HttpContext;
using Learning.Web.Impl.Presentation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using MudBlazor.Services;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        #region External Services: Mudblazor
        builder.Services.AddMudServices();
        #endregion

        #region BlazorInteractivity
        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();
        #endregion

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


                options.Events = new OpenIdConnectEvents()
                {
                    OnTokenValidated = (context) =>
                    {
                        var idToken = context.SecurityToken;
                        if (idToken != null)
                        {
                            MapClaim(context, idToken, ClaimConstant.AwsRoleClaim);
                            MapClaim(context, idToken, ClaimConstant.EmailClaim);
                            MapClaim(context, idToken, ClaimConstant.IsEmailVerifiedClaim);
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        #endregion

        #region Authorization Policy
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyConstant.SuperAdminPolicy, policy =>
            {
                policy.RequireClaim(ClaimConstant.AwsRoleClaim, RoleConstant.SuperAdmin);
            });

            options.AddPolicy(PolicyConstant.AdminPolicy, policy =>
            {
                policy.RequireClaim(ClaimConstant.AwsRoleClaim, RoleConstant.Admin);
            });

            options.AddPolicy(PolicyConstant.SalesPersonPolicy, policy =>
            {
                policy.RequireClaim(ClaimConstant.AwsRoleClaim, RoleConstant.Sales);
            });
        });
        #endregion

        #region CookieConfiguration
        builder.Services.AddSingleton<CookieOidcRefresher>();
        builder.Services.AddOptions<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme).Configure<CookieOidcRefresher>((cookieOptions, refresher) =>
        {
            cookieOptions.Events.OnValidatePrincipal = context => refresher.ValidateOrRefreshCookieAsync(context, OpenIdConnectDefaults.AuthenticationScheme);
        });
        builder.Services.AddOptions<OpenIdConnectOptions>().Configure(oidcOptions =>
        {
            // Request a refresh_token.
            oidcOptions.Scope.Add(OpenIdConnectScope.OfflineAccess);
            // Store the refresh_token.
            oidcOptions.SaveTokens = true;
        });

        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<AuthenticationStateProvider, PersistingAuthenticationStateProvider>();
        #endregion

        #region MediatR
        builder.Services.AddMediatR((c) =>
        {
            c.RegisterServicesFromAssembly(typeof(Business.ServiceRegistry).Assembly);
        });
        #endregion

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


    private static void MapClaim(TokenValidatedContext context, JwtSecurityToken idToken, string claim)
    {
        var roleClaim = idToken.Claims.First(c => c.Type == claim);
        if (roleClaim != null)
        {
            var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
            claimsIdentity.AddClaim(new Claim(claim, roleClaim.Value));
        }
    }
}
