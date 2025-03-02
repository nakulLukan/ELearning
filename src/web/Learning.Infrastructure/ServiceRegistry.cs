using Learning.Business.Contracts.PaymentGateway;
using Learning.Business.Impl.Data;
using Learning.Infrastructure.Impl.PaymentGateway;
using Learning.Infrasture.Persistence;
using Learning.Shared.Application.Contracts.Communication;
using Learning.Shared.Application.Contracts.Identity;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Constants;
using Learning.Shared.Infrastructure.Impl.Communication;
using Learning.Shared.Infrastructure.Impl.Identity;
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

        services.AddDbContextFactory<ApplicationDbContext>(options =>
            options
            .UseNpgsql(connectionString));
        services.AddDbContext<ApplicationDbContext>(options =>
            options
            .UseNpgsql(connectionString), contextLifetime: ServiceLifetime.Scoped);

        services.AddTransient<IAppDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddSingleton<IAppDbContextFactory, ApplicationDbContextFactory>();
        services.AddScoped<IFileStorage, AwsStorage>();
        services.AddScoped<IExternalIdentityProvider, AwsCognitoIdentityProvider>();
        services.AddScoped<IAppPaymentGateway, RazorpayPaymentGateway>();
        services.AddScoped<ISmsService, Fast2SmsService>();
    }
}
