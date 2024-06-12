using CommunityToolkit.Maui;
using Learning.App.Constants;
using Learning.App.Environment.Authorization;
using Learning.App.Impl.Services;
using Learning.App.Pages.AccountManagement;
using Learning.App.Pages.Startup;
using Learning.Core.Contracts.Authorization;
using Learning.Core.Contracts.Services;
using Learning.Core.PageModels.AccountManagement;
using Learning.Core.PageModels.Startup;

namespace Learning.App;

public static class ServiceRegistry
{
    public static MauiAppBuilder RegisterPageModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingletonWithShellRoute<SplashPage, SplashPageModel>(NavigationRoutes.SplashPage);
        builder.Services.AddSingletonWithShellRoute<PlaygroundPage, PlaygroundPageModel>(NavigationRoutes.PlaygroundPage);
        builder.Services.AddSingletonWithShellRoute<LoginPage, LoginPageModel>(NavigationRoutes.LoginPage);

        return builder;
    }

    public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IPageService, PageService>();
        builder.Services.AddSingleton<ISessionManager, AppSessionManager>();
        return builder;
    }
}
