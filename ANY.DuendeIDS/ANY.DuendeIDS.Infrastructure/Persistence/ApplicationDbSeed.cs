using ANY.Authorization.Tools.Permissions;
using ANY.DuendeIDS.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ANY.DuendeIDS.Infrastructure.Persistence;

public static class ApplicationDbSeed
{
    public static void EnsureSeedData(WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
        // context?.Database.Migrate();

        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        Log.Debug("Seeding Data");

        if (!roleMgr.Roles.Any())
        {
            CreateRoles(roleMgr);
        }


        var admin = userMgr.FindByNameAsync("admin").Result;

        if (admin != null) return;

        CreateAdminUser(userMgr);
    }

    private static void CreateAdminUser(UserManager<ApplicationUser> userMgr)
    {
        var admin = new ApplicationUser
        {
            UserName = "admin",
            Email = "admin@gmail.com",
            ProfilePictureUrl = "https://avatars.githubusercontent.com/u/20118398?v=4"
        };

        var result = userMgr.CreateAsync(admin, "Nolose99!").Result;
        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().Description);
        }

        result = userMgr.AddToRoleAsync(admin, "Administrator").Result;
        
        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().Description);
        }

        Log.Information("User {Admin} created", admin.UserName);
    }

    private static void CreateRoles(RoleManager<IdentityRole> roleMgr)
    {
        var roles = Enum.GetNames(typeof(Roles));
        foreach (var role in roles)
        {
            var result = roleMgr.CreateAsync(new IdentityRole(role)).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            Log.Information("Role {Role} created", role);
        }
    }
}