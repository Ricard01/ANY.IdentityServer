using ANY.Authorization.Tools.Constants;
using ANY.Authorization.Tools.Permissions;
using ANY.Authorization.Tools.Policy;
using ANY.Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace ANY.Identity.Infrastructure.HostingConfiguration;

public static class AuthorizationExtension
{
    public static void ConfigAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

        services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

        // services.AddAuthorization(options =>
        // {
        //     options.AddPolicy("RequireInteractiveUser",
        //         policy => { policy.RequireClaim(PermissionConstants.ClaimType, "UserAllAccess"); });
        // });


        // To Validate Policy's
        // services.AddTransient<IAuthorizationService, CheckAuthorizationService>();
    }
}