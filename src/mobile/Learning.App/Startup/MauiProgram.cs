
using CommunityToolkit.Maui;
using Learning.Core.Helpers.Extensions;
using Microsoft.Extensions.Logging;

namespace Learning.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMediaElement()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Roboto-Bold.ttf", "RobotoBold");
                fonts.AddFont("Roboto-Medium.ttf", "RobotoMedium");
                fonts.AddFont("Roboto-Regular.ttf", "RobotoRegular");
                fonts.AddFont("fa-brands-400.ttf", "FaBrands");
                fonts.AddFont("fa-regular-400.ttf", "FaRegular");
                fonts.AddFont("fa-solid-900.ttf", "FaSolid");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.ConfigureServices();


        var app = builder.Build();
        AppServiceProvider.ServiceProvider = app.Services;
        return app;
    }
}
