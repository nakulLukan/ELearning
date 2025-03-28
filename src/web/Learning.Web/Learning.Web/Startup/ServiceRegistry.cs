﻿using System.Globalization;
using System.Threading.RateLimiting;
using Blazor.SubtleCrypto;
using Blazored.LocalStorage;
using Learning.Business;
using Learning.Business.Contracts.HttpContext;
using Learning.Business.Contracts.Persistence;
using Learning.Infrastructure;
using Learning.Shared.Common.Constants;
using Learning.Shared.Constants;
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
using Learning.Web.Contracts.Authentication;
using Learning.Web.Contracts.Events;
using Learning.Web.Impl.Authentication;
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
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
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
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Oidc:Authority"]
                };
                options.Scope.Clear();
                foreach (var scope in configuration["Oidc:Scope"]!.Split(' '))
                {
                    options.Scope.Add(scope);
                }

                options.Events = new OpenIdConnectEvents()
                {
                    OnRedirectToIdentityProvider = context =>
                    {
                        context.ProtocolMessage.RedirectUri = context.ProtocolMessage.RedirectUri.Replace("http://", "https://");
                        return Task.CompletedTask;
                    },
                };
            });

        #endregion

        #region Authorization Policy
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyConstant.AdminPolicy, policy =>
            {
                policy.RequireClaim(ClaimConstant.AwsRoleClaim);
                policy.RequireAssertion(context =>
                    !context.User.HasClaim(ClaimConstant.AwsRoleClaim, RoleConstant.User));
            });
            options.AddPolicy(PolicyConstant.CouponCodePolicy, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(ClaimConstant.AwsRoleClaim, RoleConstant.SuperAdmin)
                    || context.User.HasClaim(ClaimConstant.AwsRoleClaim, RoleConstant.Admin)
                    || context.User.HasClaim(ClaimConstant.AwsRoleClaim, RoleConstant.OfficeStaff)
                    || context.User.HasClaim(ClaimConstant.AwsRoleClaim, RoleConstant.CouponCodes));
            });
            options.AddPolicy(PolicyConstant.ContactUsRequestPolicy, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(ClaimConstant.AwsRoleClaim, RoleConstant.SuperAdmin)
                    || context.User.HasClaim(ClaimConstant.AwsRoleClaim, RoleConstant.Admin)
                    || context.User.HasClaim(ClaimConstant.AwsRoleClaim, RoleConstant.OfficeStaff)
                    || context.User.HasClaim(ClaimConstant.AwsRoleClaim, RoleConstant.Marketing));
            });
            options.AddPolicy(PolicyConstant.UserAccountPolicy, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(ClaimConstant.AwsRoleClaim, RoleConstant.SuperAdmin)
                    || context.User.HasClaim(ClaimConstant.AwsRoleClaim, RoleConstant.Admin)
                    || context.User.HasClaim(ClaimConstant.AwsRoleClaim, RoleConstant.Marketing)
                    || context.User.HasClaim(ClaimConstant.AwsRoleClaim, RoleConstant.OfficeStaff));
            });
            options.AddPolicy(PolicyConstant.ExamNotificationPolicy, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(ClaimConstant.AwsRoleClaim, RoleConstant.SuperAdmin)
                    || context.User.HasClaim(ClaimConstant.AwsRoleClaim, RoleConstant.Admin)
                    || context.User.HasClaim(ClaimConstant.AwsRoleClaim, RoleConstant.OfficeStaff)
                    || context.User.HasClaim(ClaimConstant.AwsRoleClaim, RoleConstant.ExamNotification) 
                    || context.User.HasClaim(ClaimConstant.AwsRoleClaim, RoleConstant.QuizExamNotification));
            });
            options.AddPolicy(PolicyConstant.QuizPolicy, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(ClaimConstant.AwsRoleClaim, RoleConstant.SuperAdmin)
                    || context.User.HasClaim(ClaimConstant.AwsRoleClaim, RoleConstant.Admin)
                    || context.User.HasClaim(ClaimConstant.AwsRoleClaim, RoleConstant.OfficeStaff)
                    || context.User.HasClaim(ClaimConstant.AwsRoleClaim, RoleConstant.QuizTeam)
                    || context.User.HasClaim(ClaimConstant.AwsRoleClaim, RoleConstant.QuizExamNotification));
            });
        });
        #endregion

        #region CookieConfiguration
        builder.Services.AddSingleton<CookieOidcRefresher>();
        builder.Services.AddOptions<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme).Configure<CookieOidcRefresher>((cookieOptions, refresher) =>
        {
            cookieOptions.Cookie.HttpOnly = true;
            cookieOptions.Cookie.SecurePolicy = CookieSecurePolicy.Always;

            cookieOptions.ExpireTimeSpan = TimeSpan.FromDays(10);
            cookieOptions.SlidingExpiration = true;
            cookieOptions.Events.OnValidatePrincipal = context => refresher.ValidateOrRefreshCookieAsync(context, OpenIdConnectDefaults.AuthenticationScheme, builder.Configuration[AppSettingsKeyConstant.Oidc_Domain]!);
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

        #region Rate Limiting
        static string? GetRateLimitingPartitionKey(HttpContext httpContext)
        {
            return httpContext.Request.Cookies[CookieConstant.ClientId];
        }

        builder.Services.AddRateLimiter(rateLimiterOptions =>
        {
            rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            rateLimiterOptions.OnRejected = async (context, token) =>
            {
                // Redirect users to a custom page
                context.HttpContext.Response.Redirect("/error/attempt-exceeded");
            };
            rateLimiterOptions.AddPolicy(RateLimitingPolicyConstant.SignupPage, httpContext =>
            {
                var partitionKey = GetRateLimitingPartitionKey(httpContext);
                var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
                return RateLimitPartition.GetFixedWindowLimiter(partitionKey!,
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 10,
                        Window = TimeSpan.FromMinutes(10),
                        QueueLimit = 0,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    });
            });
            rateLimiterOptions.AddPolicy(RateLimitingPolicyConstant.ConfirmAccountPage, httpContext =>
            {
                var partitionKey = GetRateLimitingPartitionKey(httpContext);
                return RateLimitPartition.GetFixedWindowLimiter(partitionKey!,
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 15,
                        Window = TimeSpan.FromMinutes(15),
                        QueueLimit = 0,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    });
            });
            rateLimiterOptions.AddPolicy(RateLimitingPolicyConstant.ForgotPasswordPage, httpContext =>
            {
                var partitionKey = GetRateLimitingPartitionKey(httpContext);
                return RateLimitPartition.GetFixedWindowLimiter(partitionKey!,
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 10,
                        Window = TimeSpan.FromMinutes(10),
                        QueueLimit = 0,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    });
            });
        });

        #endregion

        builder.Services.AddBlazorBootstrap();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddControllersWithViews();
        builder.Services.AddAntiforgery();
        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddSubtleCrypto();
        builder.Services.AddHttpClient();
        builder.Services.AddProblemDetails();
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
        builder.Services.AddScoped<IUserManager, UserManager>();

        builder.Services.AddScoped<IQuizDataService, QuizDataService>();
        builder.Services.AddScoped<IContactUsDataService, ContactUsDataService>();
        builder.Services.AddScoped<ICouponCodeDataService, CouponCodeDataService>();
        builder.Services.AddScoped<IExamNotificationDataService, ExamNotificationDataService>();
        builder.Services.AddScoped<IModelExamDataService, ModelExamDataService>();
        builder.Services.AddScoped<IBrowserStorage, BrowserLocalStorage>();
        builder.Services.AddTransient<IAppJSInterop, AppJSInterop>();
    }
}
