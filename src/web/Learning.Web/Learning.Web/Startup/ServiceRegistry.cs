using Blazor.SubtleCrypto;
using Blazored.LocalStorage;
using Learning.Business;
using Learning.Business.Contracts.HttpContext;
using Learning.Business.Contracts.Persistence;
using Learning.Infrastructure;
using Learning.Shared.Common.Constants;
using Learning.Shared.Contracts.HttpContext;
using Learning.Web.Client.Constants;
using Learning.Web.Client.Contracts.Interop;
using Learning.Web.Client.Contracts.Persistance;
using Learning.Web.Client.Contracts.Presentation;
using Learning.Web.Client.Contracts.Services.DataCollection;
using Learning.Web.Client.Contracts.Services.ExamNotification;
using Learning.Web.Client.Contracts.Services.Quiz;
using Learning.Web.Client.Contracts.Services.Subscription;
using Learning.Web.Client.Impl.Interop;
using Learning.Web.Client.Impl.Persistance;
using Learning.Web.Client.Impl.Presentation;
using Learning.Web.Client.Services.Quiz;
using Learning.Web.Contracts.Events;
using Learning.Web.Impl.Events;
using Learning.Web.Impl.HttpContext;
using Learning.Web.Impl.Persistence;
using Learning.Web.Impl.Presentation;
using Learning.Web.Services.DataCollection;
using Learning.Web.Services.ExamNotification;
using Learning.Web.Services.Quiz;
using Learning.Web.Services.Subscription;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
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
        builder.Services.RegisterBusinessServices();
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
                options.RequireHttpsMetadata = true;
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
                    OnRedirectToIdentityProvider = context =>
                    {
                        context.ProtocolMessage.RedirectUri = context.ProtocolMessage.RedirectUri.Replace("http://", "https://");
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
            cookieOptions.ExpireTimeSpan = TimeSpan.FromDays(10);
            cookieOptions.SlidingExpiration = true;
            cookieOptions.Events.OnSigningIn = context =>
            {
                context.Properties.IsPersistent = true;
                return Task.CompletedTask;
            };
            cookieOptions.Events.OnValidatePrincipal = context => refresher.ValidateOrRefreshCookieAsync(context, OpenIdConnectDefaults.AuthenticationScheme);
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

        #region CORS
        builder.Services.AddCors((options) =>
        {
            options.AddPolicy("Default", policy =>
            {
                policy.WithOrigins("https://localhost:5000");
            });
        });
        #endregion CORS

        builder.Services.AddBlazorBootstrap();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddControllersWithViews();
        builder.Services.AddAntiforgery();
        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddSubtleCrypto();
        builder.Services.AddHttpClient();
        RegisterAppServices(builder);
    }

    private static void RegisterAppServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IToastService, ToastService>();
        builder.Services.AddScoped<IAppMediator, AppMediator>();
        builder.Services.AddScoped<IRequestContext, RequestContextServer>();
        builder.Services.AddScoped<IApiRequestContext, ApiRequestContext>();
        builder.Services.AddSingleton<IAppCache, IAppMemoryCache>();
        builder.Services.AddScoped<IQuizManager, QuizManager>();
        builder.Services.AddScoped<IAlertService, AlertService>();
        builder.Services.AddScoped<INavigationService, NavigationService>();

        builder.Services.AddScoped<IQuizDataService, QuizDataService>();
        builder.Services.AddScoped<IContactUsDataService, ContactUsDataService>();
        builder.Services.AddScoped<ICouponCodeDataService, CouponCodeDataService>();
        builder.Services.AddScoped<IExamNotificationDataService, ExamNotificationDataService>();
        builder.Services.AddScoped<IModelExamDataService, ModelExamDataService>();
        builder.Services.AddScoped<IBrowserStorage, BrowserLocalStorage>();
        builder.Services.AddTransient<IAppJSInterop, AppJSInterop>();
    }
}
