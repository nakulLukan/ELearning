using Learning.Shared.Common.Constants;
using Learning.Shared.Common.Extensions;
using Microsoft.AspNetCore.DataProtection;

namespace Learning.App.Web;

public static class HostBuilderExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(opt =>
        {
            opt.DefaultScheme = "cookie";
            opt.DefaultChallengeScheme = "oidc";
        })
            .AddCookie("cookie", opt =>
            {
                opt.Cookie.Name = "web.app.cookie";
            })
            .AddOpenIdConnect("oidc", opt =>
            {
                opt.Authority = configuration[AppSettingsKeyConstant.IdentityServer_Authority];
                opt.ClientId = IdentityClientConstant.WebAppClient;
                opt.ClientSecret = "public-blazor-client-secret";

                opt.ResponseType = "code";
                opt.ResponseMode = "query";

                opt.Scope.Clear();
                opt.Scope.Add("openid");
                opt.Scope.Add("profile");
                opt.Scope.Add("offline_access");
                opt.Scope.Add("api");

                opt.MapInboundClaims = false;
                opt.GetClaimsFromUserInfoEndpoint = true;
                opt.SaveTokens = true;
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role",
                };
            });
        services.AddAccessTokenManagement();
        return services;
    }
}
