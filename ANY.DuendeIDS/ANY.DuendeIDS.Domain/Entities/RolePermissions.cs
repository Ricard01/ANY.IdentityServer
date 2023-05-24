using System.ComponentModel.DataAnnotations;
using ANY.Authorization.Tools.Permissions;

namespace ANY.DuendeIDS.Domain.Entities;

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
    private readonly string _permissionsInRole = null!;
    // /// <summary>
    // /// This returns the list of permissions in this role
    // /// </summary>
    public IEnumerable<Permissions> PermissionsInRole => _permissionsInRole.UnpackPermissionsFromString(); 

}