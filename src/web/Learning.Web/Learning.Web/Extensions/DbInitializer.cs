using Learning.Business.Impl.Data;
using Learning.Infrastructure.Data.Seeder;
using Learning.Infrasture.Persistence;
using Learning.Shared.Models.Identity;
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

                var roleSeeder = scope.ServiceProvider.GetRequiredService<RoleSeeder>();
                await roleSeeder.Seed();

                var userSeeder = scope.ServiceProvider.GetRequiredService<AdminUserSeeder>();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var defaultUsers = configuration.GetSection("Identity:AdminUsers").Get<List<DefaultUserDto>>()!;
                await userSeeder.Seed(defaultUsers);
            }
        });
        return Task.CompletedTask;
    }
}
