using Microsoft.AspNetCore.Identity;

namespace ANY.DuendeIDS.Domain.Entities;

public class ApplicationRole : IdentityRole<Guid>
{
    /// <summary>
    /// Permissions separated by commas  ( this will be the Claims)
    /// 
    /// </summary>
    public string Permissions { get; set; } = null!;
}