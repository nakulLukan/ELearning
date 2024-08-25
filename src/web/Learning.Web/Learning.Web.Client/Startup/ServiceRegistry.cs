using Blazor.SubtleCrypto;
using Blazored.LocalStorage;
using Learning.Shared.Contracts.HttpContext;
using Learning.Web.Client.Contracts.Interop;
using Learning.Web.Client.Contracts.Persistance;
using Learning.Web.Client.Contracts.Presentation;
using Learning.Web.Client.Contracts.Relay;
using Learning.Web.Client.Contracts.Services.DataCollection;
using Learning.Web.Client.Contracts.Services.Quiz;
using Learning.Web.Client.Contracts.Services.Subscription;
using Learning.Web.Client.Impl.HttpContext;
using Learning.Web.Client.Impl.Interop;
using Learning.Web.Client.Impl.Persistance;
using Learning.Web.Client.Impl.Relay;
using Learning.Web.Client.Services.DataCollection;
using Learning.Web.Client.Services.Quiz;
using Learning.Web.Client.Services.Subscription;
using Learning.Web.Client.Services.WebServices;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Refit;

namespace Learning.Web.Client;

internal static class ServiceRegistry
{
    public static void RegisterWebServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
        builder.Services.AddSubtleCrypto(config =>
        {
            config.Key = AppSettings.EncryptionKey;
        });

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        #region Refit
        builder.Services.AddRefitClient<IPublicQuizRestService>()
                        .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
        builder.Services.AddRefitClient<IDataCollectionRestService>()
                        .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
        #endregion

        builder.Services.AddMudServices();
        builder.Services.AddBlazoredLocalStorageAsSingleton();
    }

    public static void RegisterAppServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<IRequestContext, RequestContext>();
        builder.Services.AddScoped<IBrowserStorage, BrowserLocalStorage>();
        builder.Services.AddTransient<IAppJSInterop, AppJSInterop>();
        builder.Services.AddTransient<IHttpClientService, HttpClientService>();
        builder.Services.AddScoped<IQuizManager, QuizManager>();
        builder.Services.AddSingleton<IAlertService, AlertService>();

        builder.Services.AddTransient<IQuizDataService, QuizRestDataService>();
        builder.Services.AddTransient<ICouponCodeDataService, CouponCodeRestDataService>();
        builder.Services.AddTransient<IContactUsDataService, ContactUsRestDataService>();
    }
}
