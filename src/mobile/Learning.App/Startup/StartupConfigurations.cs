using IdentityModel.OidcClient;
using Learning.App.Environment.Authorization;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Reflection;

namespace Learning.App;

public static class StartupConfigurations
{
    public static void ConfigureServices(this MauiAppBuilder builder)
    {
        #region Logger
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(Path.Combine(FileSystem.AppDataDirectory + "/logs", "logs.txt"), rollingInterval: RollingInterval.Day)
            .CreateLogger();
        #endregion Logger

        #region AppSettings.json
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Learning.App.appsettings.json");
        var config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();
        builder.Configuration.AddConfiguration(config);
        #endregion AppSettings.json

        #region Add Oidc Client

        var configuration = builder.Configuration;
        builder.Services.AddSingleton(new OidcClient(new()
        {
            Authority = configuration["Oidc:Authority"],
            ClientId = configuration["Oidc:ClientId"],
            ClientSecret = configuration["Oidc:ClientSecret"],
            Scope = configuration["Oidc:Scope"],
            RedirectUri = configuration["Oidc:RedirectUri"],
            Browser = new AuthenticationBrowser(),
            Policy = new Policy()
            {
                Discovery = new IdentityModel.Client.DiscoveryPolicy()
                {
                    ValidateEndpoints = false
                }
            },
        }));
        #endregion

        #region AppServices
        builder.RegisterAppServices();
        #endregion AppServices

        #region Pages
        builder.RegisterPageModels();
        #endregion
    }
}
