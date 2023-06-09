using Microsoft.AspNetCore.Identity;

namespace ANY.DuendeIDS.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string Name { get; set; } = null!;
    public string? ProfilePictureUrl { get; init; }

    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
}