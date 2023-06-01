using Microsoft.AspNetCore.Authorization;

namespace ANY.Authorization.Tools.Permissions;

public class PermissionRequirement : IAuthorizationRequirement
{
    
    public string PermissionName { get; }

    public PermissionRequirement(string permissionName)
    {
        PermissionName = permissionName ?? throw new ArgumentNullException(nameof(permissionName));
    }
    // public static string ClaimType => AppClaimTypes.Permissions;
    //
    // public PermissionOperator PermissionOperator { get; }
    //
    // public string[] Permissions { get; }

    // public PermissionRequirement(PermissionOperator permissionOperator, string permissionName)
    // {
    //     if (permissions.Length == 0)
    //         throw new ArgumentException("At least one permission is required.", nameof(permissions));
    //
    //     PermissionOperator = permissionOperator;
    //     Permissions = permissions;
    // }
}