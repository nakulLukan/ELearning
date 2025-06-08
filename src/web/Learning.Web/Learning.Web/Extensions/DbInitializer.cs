using Learning.Business.Impl.Data;
using Learning.Infrasture.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Learning.Web.Extensions;
public static class DbInitializer
{
    public static Task RunMigrationAsync(this WebApplication app)
    {
        Task.Run(async () =>
        {
            try
            {
                using var scope = app.Services.CreateScope();
                var dbContext = (ApplicationDbContext)scope.ServiceProvider.GetRequiredService<IAppDbContextFactory>().CreateDbContext();
                await dbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Migration failed");
            }
        });
        return Task.CompletedTask;
    }
}