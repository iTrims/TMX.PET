using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4;

namespace IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("TMXWebApi", "Web API")
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("TMXWebApi", "Web API", new []{JwtClaimTypes.Name})
                {
                    Scopes = {"TMXWebApi" }
                }
            };
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "tmx-web-app",
                    ClientName = "TMX Web",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    RedirectUris =
                    {
                        "https://localhost:7062/swagger/oauth2-redirect.html"
                    },
                    AllowedCorsOrigins =
                    {
                        "https://localhost:7062", "http://localhost:5168"
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:7062/swagger/index.html"
                        //"http://localhost:7062/signout-oidc"// while there is no application client
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "TMXWebApi"
                    },
                    AllowAccessTokensViaBrowser = true
                }
            };
    }

}
