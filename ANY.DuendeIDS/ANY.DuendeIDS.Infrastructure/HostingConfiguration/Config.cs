using Duende.IdentityServer.Models;

namespace ANY.DuendeIDS.Infrastructure.HostingConfiguration;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {   
            // admin users CRUD
            new ApiScope("identity.api")
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new ApiResource("identityApi")
            {
                Scopes = new List<string> { "identity.api" },
                ApiSecrets = new List<Secret> { new Secret("ScopeSecret".Sha256()) }
            }
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "bffAngular",
                ClientName = "BFF Interactive Client (Code with PKCE)",

                RedirectUris = { "https://localhost:44473" },
                PostLogoutRedirectUris = { "https://localhost:44473" },
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                AllowedScopes = AllScopes,
                
                
                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.ReUse,
                RefreshTokenExpiration = TokenExpiration.Sliding,
            },

            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "interactive",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:44300/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "scope2" }
            },
        };

    private static IEnumerable<string> AllIdentityScopes =>
        IdentityResources.Select(s => s.Name).ToList();

    private static List<string> AllApiScopes =>
        ApiScopes.Select(s => s.Name).ToList();

    private static List<string> AllScopes =>
        AllApiScopes.Concat(AllIdentityScopes).ToList();
}