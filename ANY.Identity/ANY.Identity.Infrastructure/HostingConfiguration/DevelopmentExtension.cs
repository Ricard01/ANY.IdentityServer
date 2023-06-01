using ANY.Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace ANY.Identity.Infrastructure.HostingConfiguration;

public static class DevelopmentExtension
{

    public static void ConfDevelopment(this IServiceCollection services)
    {
        // services.AddTransient<IAuthorizationService, CheckAuthorizationService>();
        
        // services.AddSingleton<IAuthorizationMiddlewareResultHandler,
        //     MyAuthorizationMiddlewareResultHandler>();
    }
}