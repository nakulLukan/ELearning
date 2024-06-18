using Learning.Business.Impl.Data;
using Learning.Infrasture.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Learning.Web.Extensions;

public static class DbInitializer
{
    public static Task RunMigrationAsync(this WebApplication app)
    {
        Task.Run(async () =>
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IAppDbContextFactory>().CreateDbContext() as ApplicationDbContext;
                await dbContext.Database.MigrateAsync();
            }
        });
        return Task.CompletedTask;
    }
}
