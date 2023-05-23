using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Any.AuthorizationTools.Permissions;

namespace Any.AuthorizationTools.Entities;

public class RolePermissions
{
    /// <summary>
    /// ShortName of the role
    /// </summary>
    [Key]
    [MaxLength(100)]
    public string RoleName { get; private set; } = null!;

    /// <summary>
    /// A human-friendly description of what the Role does
    /// 
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string Description { get; private set; }  = null!;

    
    [Required(AllowEmptyStrings = false)] //A role must have at least one role in it
    private string _permissionsInRole;
    // /// <summary>
    // /// This returns the list of permissions in this role
    // /// </summary>
    public IEnumerable<Permissions.Permissions> PermissionsInRole => _permissionsInRole.UnpackPermissionsFromString(); 
}