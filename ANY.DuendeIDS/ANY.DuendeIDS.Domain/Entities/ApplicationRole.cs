using ANY.Authorization.Tools.Permissions;
using Microsoft.AspNetCore.Identity;

namespace ANY.DuendeIDS.Domain.Entities;

public class ApplicationRole : IdentityRole<Guid>
{
    // private readonly string _permissions;


    /// <summary>
    /// A human-friendly description of what the Role does
    /// </summary>
    public string? Description { get; init; }

    public string? Permissions { get; init; }

    // public IList<Permissions> Permissions { get; init; } = new List<Permissions>();

    // /// <summary>
    // /// This returns the list of permissions in this role
    // /// </summary>
    // public IEnumerable<Permissions> Permissions => _permissions.UnpackPermissionsFromString();
}