using System.Security.Claims;
using ANY.DuendeIDS.Domain.Entities;
using ANY.DuendeIDS.Infrastructure.Persistence;
using ANY.DuendeIDS.Infrastructure.Services;
using Duende.IdentityServer;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Hosting.LocalApiAuthentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace ANY.DuendeIDS.Infrastructure.HostingExtensions;

public static class IdentityExtension
{
    public static void IdentityConfiguration(this IServiceCollection services)
    {
        // Order matter this is the correct order of services. 
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            // .AddClaimsPrincipalFactory<AppUserClaimsPrincipalFactory>() Doesnt work with OpendIdConnect 
            .AddEntityFrameworkStores<ApplicationDbContext>();
            // .AddDefaultTokenProviders();

        services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                // options.EmitStaticAudienceClaim = true;  // emits audience resource
            })
            .AddInMemoryIdentityResources(IDSConfig.IdentityResources)
            .AddInMemoryApiScopes(IDSConfig.ApiScopes)
            .AddInMemoryApiResources(IDSConfig.ApiResources) // Provides Audience Validation on Token
            .AddInMemoryClients(IDSConfig.Clients)
            .AddAspNetIdentity<ApplicationUser>()
            .AddProfileService<MyProfileService>();

        // adds an authentication handler that validates incoming tokens using IdentityServer’s built-in token validation engine
        // configures the authentication handler to require a scope claim inside the access token of value IdentityServerApi
        // sets up an authorization policy that checks for a scope claim of value IdentityServerApi
        services.AddLocalApiAuthentication(principal =>
        {
            principal.Identities.First().AddClaim(new Claim("additional_claim", "additional_value"));

            return Task.FromResult(principal);
        });

        services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "IdentityServer.Cookie";
                config.Cookie.SameSite = SameSiteMode.None;
                config.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            }
        );

        // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //     .AddCookie(config =>
        //         {
        //             config.Cookie.Name = "IdentityServer.Cookie";
        //             config.Cookie.SameSite = SameSiteMode.None;
        //             config.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        //         }
        //     )
        //     .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        //     {
        //         options.Authority = "https:localhost:5000";
        //         options.Audience = "identityApi";
        //
        //         options.MapInboundClaims = true;
        //         // it's recommended to check the type header to avoid "JWT confusion" attacks
        //         options.TokenValidationParameters = new TokenValidationParameters()
        //         {
        //             ValidTypes = new[] { "at+jwt" }
        //         };
        //         options.RequireHttpsMetadata = false;
        //         options.Configuration = new OpenIdConnectConfiguration();
        //         options.Events = new JwtBearerEvents()
        //         {
        //             OnAuthenticationFailed = msg =>
        //             {
        //                 var ok = msg;
        //                 Console.WriteLine("OnAuthenticationFailed");
        //                 return Task.CompletedTask;
        //             },
        //             OnForbidden = msg =>
        //             {
        //                 var ok = msg;
        //                 Console.WriteLine("OnForbidden");
        //                 return Task.CompletedTask;
        //             },
        //
        //             OnMessageReceived = msg =>
        //             {
        //                 var token = msg?.Request.Headers.Authorization.ToString();
        //                 string path = msg?.Request.Path ?? "";
        //                 if (!string.IsNullOrEmpty(token))
        //
        //                 {
        //                     Console.WriteLine("Access token");
        //                     Console.WriteLine($"URL: {path}");
        //                     Console.WriteLine($"Token: {token}\r\n");
        //                 }
        //                 else
        //                 {
        //                     Console.WriteLine("Access token");
        //                     Console.WriteLine("URL: " + path);
        //                     Console.WriteLine("Token: No access token provided\r\n");
        //                 }
        //
        //                 return Task.CompletedTask;
        //             },
        //             // claims that the handler received extracted from the access token
        //             OnTokenValidated = ctx =>
        //             {
        //                 Console.WriteLine();
        //                 Console.WriteLine("Claims from the access token");
        //                 if (ctx?.Principal != null)
        //                 {
        //                     foreach (var claim in ctx.Principal.Claims)
        //                     {
        //                         Console.WriteLine($"{claim.Type} - {claim.Value}");
        //                     }
        //                 }
        //
        //                 Console.WriteLine();
        //                 return Task.CompletedTask;
        //             }
        //         };
        //     });

        services.AddAuthorization(options =>
        {
            // So that an empty [Authorize] attribute applies different policies
            // Note: Even when in the controller is specified another Role for an action wont work but a policy will
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                // .RequireClaim("scope", IdentityServerConstants.LocalApi.ScopeName)
                .Build();
        });
    }
}