using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace ANY.DuendeIDS.Infrastructure.HostingExtensions;

public static class IDSConfig
{
    // The Identity scopes control what goes into the ID token
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };


// The access scopes control what APIs and services the client application wants to access and what should go into the access token
    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            // admin users CRUD
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName),
            new ApiScope("remoteApi")
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new ApiResource("identityApi")
            {
                Scopes = new List<string> { IdentityServerConstants.LocalApi.ScopeName, "remoteApi" },
                ApiSecrets = new List<Secret> { new Secret("ScopeSecret".Sha256()) }
            },

            // Resource Isolation This is an Enterprise Edition feature.
            new ApiResource("urn:remoteApi")
            {
                // f you have API resources, where you want to make sure they are not sharing access tokens with other resources, you can enforce the resource indicator
                RequireResourceIndicator = true,
                Scopes = new List<string> { "remoteApi" },
                ApiSecrets = new List<Secret> { new Secret("ScopeSecret".Sha256()) }
            }
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "spa",
                ClientSecrets = { new Secret("secret".Sha256()) },


                RedirectUris = { "https://localhost:5002" },
                PostLogoutRedirectUris = { "https://localhost:5002" },


                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                //The list of scopes represents a mixed list of what the client wants to get back
                AllowedScopes = AllScopes,
                // AlwaysIncludeUserClaimsInIdToken = true,


                RefreshTokenUsage = TokenUsage.ReUse,
                RefreshTokenExpiration = TokenExpiration.Sliding,

                // AllowedGrantTypes = 
                // { 
                //     GrantType.AuthorizationCode, 
                //     GrantType.ClientCredentials,
                //     OidcConstants.GrantTypes.TokenExchange 
                // },

                // RedirectUris = { "https://localhost:5002/signin-oidc" },

                //FrontChannelLogoutUri = "https://localhost:5002/signout-oidc",
                // BackChannelLogoutUri = "https://localhost:5002/bff/backchannel",

                // PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                AllowOfflineAccess = true,
                // AllowedScopes = { "openid", "profile", "IdentityServerApi",  },
            },
            new Client
            {
                ClientId = "bffTest",
                ClientName = "BFF Test)",

                RedirectUris = { "https://localhost:44451" },
                BackChannelLogoutUri = "https://localhost:44451",
                PostLogoutRedirectUris = { "https://localhost:44451" },
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                //The list of scopes represents a mixed list of what the client wants to get back
                AllowedScopes = AllScopes,


                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.ReUse,
                RefreshTokenExpiration = TokenExpiration.Sliding,
            },
            new Client
            {
                ClientId = "bffAngular",
                ClientName = "BFF Interactive Client (Code with PKCE)",

                RedirectUris = { "https://localhost:44473" },
                PostLogoutRedirectUris = { "https://localhost:44473" },
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                //The list of scopes represents a mixed list of what the client wants to get back
                AllowedScopes = AllScopes,
                // AlwaysIncludeUserClaimsInIdToken = true,
                AllowedCorsOrigins = { "https://localhost:44473" },

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