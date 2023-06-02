using System.Security.Claims;
using ANY.Authorization.Tools;
using ANY.DuendeIDS.Domain.Entities;
using ANY.DuendeIDS.Infrastructure.Persistence;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace ANY.DuendeIDS.Infrastructure.Services;

public class MyProfileService : IProfileService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;

    public MyProfileService(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory) //TestUserStore users
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
    }

    // GetProfileDataAsync is what controls what claims are issued in the response
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        // context holds information about the request, the user, the client, the scopes, and the claims being requested
        // context.Subject is the user for whom the result is request is being made
        // context.Subject.Claims is the claims collection from the user's session cookie at login time
        // context.IssuedClaims is the collection of claims that your logic has decided to return in the response


        // UserId From Token
        var sub = context.Subject.GetSubjectId();

        var user = await _userManager.FindByIdAsync(sub);

        var userClaims = await _userClaimsPrincipalFactory.CreateAsync(user);

        List<Claim> claims = userClaims.Claims.ToList();
        claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();


        if (_userManager.SupportsUserRole)
        {
            var roles = await _userManager.GetRolesAsync(user);

       

            // I Will only allow one rol per user because lets imagine the next scenario: 
            // User : Supervisor Mexico Zona A | Roles : RolMexCityA , RolMexCityB, RolMexCitiC (this will save time if the permissions
            // are exactly what the user needs but if a single permissions is no need it then we have a create a different rol lets say
            // RoleMexCitaAWithouCancelPermission , well it will be cleaner just create a single rol named SupervisorZoneA that 3 different Roles without X Perm)
            // in my experience is easier to manage single rol per user than same roles with kinda similar permissions quicky gets messy


            foreach (var role in roles)
            {
                var permInRole = await _context.Roles.Where(r => r.Name == role)
                    .Select(r => r.Permissions).FirstOrDefaultAsync();


                if (permInRole != null) claims.Add(new Claim(Constants.ClaimType, permInRole));
            }
        }

        context.IssuedClaims = claims;


        // return IssueTask.CompletedTask;
    }

    // IsActiveAsync is called to ask your custom logic if the user is still "active".
    // If the user is not "active" then no new tokens will be created for them, even 
    // if the user has an active session with IdentityServer.
    public Task IsActiveAsync(IsActiveContext context)
    {
        // as above, context.Subject is the user for whom the result is request is being made
        // setting context.IsActive to false allows your logic to indicate that the token should not be created
        // context.IsActive defaults to true

        return Task.CompletedTask;
    }

    
}