using Blazored.LocalStorage;
using Learning.Business;
using Learning.Business.Contracts.HttpContext;
using Learning.Business.Contracts.Persistence;
using Learning.Web.Client.Contracts.Interop;
using Learning.Web.Client.Contracts.Persistance;
using Learning.Web.Client.Impl.HttpContext;
using Learning.Web.Client.Impl.Interop;
using Learning.Web.Client.Impl.Persistance;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

namespace Learning.Web.Client;

internal static class ServiceRegistry
{
    public static void RegisterWebServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();


        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        builder.Services.AddMudServices();
        builder.Services.AddBlazoredLocalStorageAsSingleton();
    }

    public static void RegisterAppServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<IRequestContext, RequestContext>();
        builder.Services.AddScoped<IBrowserStorage, BrowserLocalStorage>();
        builder.Services.AddTransient<IAppJSInterop, AppJSInterop>();
        builder.Services.RegisterBusinessServices();
    }
}
