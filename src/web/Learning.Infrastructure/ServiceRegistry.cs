using Learning.Business.Impl.Data;
using Learning.Infrasture.Persistence;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Constants;
using Learning.Shared.Infrastructure.Impl.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Learning.Infrastructure;

public static class ServiceRegistry
{
    public static void RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration[AppSettingsKeyConstant.ConnectionStrings_Default];

        services.AddDbContext<ApplicationDbContext>(options =>
            options
            .UseNpgsql(connectionString), contextLifetime: ServiceLifetime.Transient);
        services.AddTransient<IAppDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IFileStorage, AwsStorage>();
    }
}
