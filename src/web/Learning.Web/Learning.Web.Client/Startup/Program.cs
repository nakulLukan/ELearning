using Learning.Web.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RegisterWebServices();
builder.RegisterAppServices();
await builder.Build().RunAsync();
