using Microsoft.AspNetCore.Authorization;
using ANY.Authorization.Tools.Constants;

namespace ANY.Authorization.Tools.Permissions;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var permissionsClaim =
            context.User.Claims.SingleOrDefault(c => c.Type == PermissionConstants.ClaimType);
        // If user does not have the scope claim, get out of here
        if (permissionsClaim == null)
            return Task.CompletedTask;

        if (permissionsClaim.Value.ThisPermissionIsAllowed(requirement.PermissionName))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
