using ANY.Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace ANY.Identity.Infrastructure.HostingConfiguration;

public static class AuthorizationExtension
{
    public static void ConfigAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
        
            options.AddPolicy("RequireInteractiveUser", policy =>
            {
                    policy.RequireClaim("Permissions", "hola");
            });
        });


        // To Validate Policy's
        services.AddTransient<IAuthorizationService, CheckAuthorizationService>();
    }
}