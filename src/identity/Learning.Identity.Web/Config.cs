using Duende.IdentityServer.Models;
using Learning.Shared.Common.Constants;

namespace Learning.Identity.Web;

public static class Config
{
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };
    }

    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return new List<ApiScope>
        {
            new ApiScope(name: "api",   displayName: "My Web APIs"),
        };
    }


    public static IEnumerable<Client> GetClients()
    {
        return new List<Client>
        {
            new Client()
            {
                ClientId = IdentityClientConstant.WebAppClient,
                AllowedGrantTypes = GrantTypes.Code,
                AllowOfflineAccess = true,
                ClientSecrets = { new Secret("public-blazor-client-secret".Sha256())},
                RequireClientSecret = true,
                RedirectUris =  { "http://localhost:7000/signin-oidc", "https://localhost:7001/signin-oidc" },
                PostLogoutRedirectUris = { "http://localhost:7000/","https://localhost:7001/" },
                FrontChannelLogoutUri =    "https://localhost:7001/signout-oidc",
                AllowedScopes = new[]
                {
                    new IdentityResources.OpenId().Name,
                    new IdentityResources.Profile().Name,
                    "api"
                }
            }
        };
    }
}
