using Microsoft.AspNetCore.Authorization;

namespace ANY.Authorization.Tools.Permissions;

public class RequiresPermissionAttribute : AuthorizeAttribute
{
    public RequiresPermissionAttribute(Permissions permission) : base(permission.ToString())
    {
    }
}