using Microsoft.Extensions.DependencyInjection;

namespace ANY.Identity.Infrastructure.HostingConfiguration;

public static class AuthenticationExtension
{
    public static void ConfigAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://localhost:5001";
                options.Audience = "identityApi";

                options.MapInboundClaims = false;
            });
    }
}