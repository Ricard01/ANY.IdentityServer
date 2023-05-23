using ANY.DuendeIDS.Domain.Entities;
using ANY.DuendeIDS.Infrastructure.Persistence;
using ANY.DuendeIDS.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ANY.DuendeIDS.Infrastructure.HostingConfiguration;

public static class IdentityExtension
{
    public static void ConfigIdentityServer(this IServiceCollection services)
    {
        // Orden Comprobado y Correcto
        services.AddIdentity<ApplicationUser, IdentityRole>()
            // Doesnt work with OpendIdConnect .AddClaimsPrincipalFactory<AppUserClaimsPrincipalFactory>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                // options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryApiResources(Config.ApiResources) // Provides Audience Validation on Token
            .AddInMemoryClients(Config.Clients)
            .AddAspNetIdentity<ApplicationUser>()
            .AddProfileService<MyProfileService>();
         // Orden Matters
    }
}