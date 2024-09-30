using Functions.Identity.Core.Contracts.ManageUsers;
using Functions.Identity.Core.Impl.ManageUsers;
using Microsoft.Extensions.DependencyInjection;

namespace CognitoPostSignUp;

internal class Startup
{
    public static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // Register your services here
        services.AddTransient<IIdentityUserManager, IdentityUserManager>();

        return services.BuildServiceProvider();
    }
}
