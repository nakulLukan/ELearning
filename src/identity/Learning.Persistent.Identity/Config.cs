using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Learning.Persistent.Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api", "E-Learning API Service"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "interactive",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    
                    RedirectUris = { "https://localhost:5020/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:5020/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:5020/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "api" }
                },
                new Client
                {
                    ClientId = "blazorWASM",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedCorsOrigins = { "https://localhost:5020" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api"
                    },
                    RedirectUris = { "https://localhost:5020/authentication/login-callback" },
                    PostLogoutRedirectUris = { "https://localhost:5020/authentication/logout-callback" },
                    AccessTokenLifetime = 60,
                }
            };
    }
}
