using Learning.Business.Contracts.HttpContext;
using Learning.Web.Client.Impl.HttpContext;
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
    }

    public static void RegisterAppServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<IRequestContext, RequestContext>();
    }
}
