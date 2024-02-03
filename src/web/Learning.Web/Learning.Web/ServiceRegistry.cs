using Learning.Business;
using Learning.Business.Contracts.HttpContext;
using Learning.Infrastructure;
using Learning.Infrastructure.Data.Seeder;
using Learning.Infrasture.Persistence;
using Learning.Web.Client.Constants;
using Learning.Web.Client.Contracts.Events;
using Learning.Web.Client.Contracts.Presentation;
using Learning.Web.Client.Impl.Events;
using Learning.Web.Client.Impl.HttpContext;
using Learning.Web.Client.Impl.Presentation;
using Learning.Web.Components.Account;
using Learning.Web.Events;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
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
        builder.Configuration.AddConfiguration(new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>()
                .Build());
        builder.Services.AddMudServices();
        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        builder.Services.AddScoped<ApplicationCookieAuthenticationEvents>();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<IdentityUserAccessor>();
        builder.Services.AddScoped<IdentityRedirectManager>();
        builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentitySchemeConstant.DefaultScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
            .AddIdentityCookies(opt =>
            {
                opt.ApplicationCookie.Configure(con =>
                {
                    con.EventsType = typeof(ApplicationCookieAuthenticationEvents);
                });
            });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyConstant.AdminPolicy, asd =>
            {
                asd.Requirements.Add(new RolesAuthorizationRequirement([RoleConstant.SuperAdmin, RoleConstant.Admin]));
            });

            options.AddPolicy(PolicyConstant.UserPolicy, pol =>
            {
                pol.Requirements.Add(new RolesAuthorizationRequirement([RoleConstant.User]));
            });
        });
        builder.Services.AddIdentityCore<Learning.Domain.Identity.ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedEmail = false;

            // Password policy
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 4;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireDigit = false;
        })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        builder.Services.AddSingleton<IEmailSender<Learning.Domain.Identity.ApplicationUser>, IdentityNoOpEmailSender>();
        builder.Services.AddMediatR((c) =>
        {
            c.RegisterServicesFromAssembly(typeof(Business.ServiceRegistry).Assembly);
        });
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
        RegisterAppServices(builder);
    }

    private static void RegisterAppServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IToastService, ToastService>();
        builder.Services.AddScoped<IAppMediator, AppMediator>();
        builder.Services.AddScoped<IRequestContext, RequestContext>();

        builder.Services.AddTransient<RoleSeeder>();
        builder.Services.AddTransient<AdminUserSeeder>();
    }
}
