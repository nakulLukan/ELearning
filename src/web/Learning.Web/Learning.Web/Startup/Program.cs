using Learning.Shared.Application.Helpers;
using Learning.Web;
using Learning.Web.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;

#region Logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/app.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        fileSizeLimitBytes: 10 * 1024 * 1024,
        shared: true,
        flushToDiskInterval: TimeSpan.FromMinutes(1))
    .CreateLogger();
#endregion
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.RegisterServices();
var app = builder.Build();
await app.RunMigrationAsync();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRateLimiter();
app.UseAntiforgery();
app.RegisterMiddlewares();
app.UseCors("Default");
app.MapControllers();
app.MapRazorComponents<Learning.Web.Components.App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Learning.Web.Client._Imports).Assembly);
app.Run();
