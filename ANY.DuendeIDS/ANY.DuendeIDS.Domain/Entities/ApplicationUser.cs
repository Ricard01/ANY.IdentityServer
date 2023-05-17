using Microsoft.AspNetCore.Identity;

namespace ANY.DuendeIDS.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string? ProfilePictureUrl { get; set; }
}