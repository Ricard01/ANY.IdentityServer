using System.Security.Claims;
using ANY.DuendeIDS.Domain.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ANY.DuendeIDS.Infrastructure.Services;

public class AppUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
{
    public AppUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,
        IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
    {
    }


    // Added on login. 
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var claimsIdentity = await base.GenerateClaimsAsync(user);


        claimsIdentity.AddClaim(new Claim("Permission", "hola"));
        claimsIdentity.AddClaim(new Claim("Permission", "hola2"));


        return claimsIdentity;
    }
}