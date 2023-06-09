
using System.Diagnostics.CodeAnalysis;
using ANY.Authorization.Tools;
using ANY.DuendeIDS.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ANY.DuendeIDS.Infrastructure.Persistence;

public static class ApplicationDbSeed
{
    private const string AdminRole = "Administrator";
    private const string AdminUser = "Admin";
    
    public static void EnsureSeedData(WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
        context?.Database.Migrate();

        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();


        Log.Debug("Seeding Data");
        
        if (!roleMgr.RoleExistsAsync(AdminRole).Result)
        {
            CreateAdminRole(roleMgr);
        }


        var admin = userMgr.FindByNameAsync(AdminUser).Result;

        if (admin != null) return;

        CreateAdminUser(userMgr);
    }

    private static void CreateAdminUser(UserManager<ApplicationUser> userMgr)
    {
        var admin = new ApplicationUser
        {
            UserName = AdminUser,
            Name = "John Doe",
            Email = "admin@gmail.com",
            ProfilePictureUrl = "https://avatars.githubusercontent.com/u/20118398?v=4"
        };

        var result = userMgr.CreateAsync(admin, "Nolose99!").Result;
        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().Description);
        }

        result = userMgr.AddToRoleAsync(admin, AdminRole).Result;

        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().Description);
        }

        Log.Information("User {Admin} created", admin.UserName);
    }

    private static void CreateAdminRole(RoleManager<ApplicationRole> roleMgr)
    {
        var permissions =  Enum.GetValues<Permissions>()
            .Where(p => Enum.GetName(typeof(Permissions), p )!.EndsWith(  "AllAccess"))
            .ToList();

        var packPermissions = permissions.Aggregate("", (s, permission) => s + (char)permission);

        var role = new ApplicationRole
        {
            Name = AdminRole,
            Description = "Administrator has access to everything",
            Permissions = packPermissions,
           
        };


        var result = roleMgr.CreateAsync(role).Result;
        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().Description);
        }

        Log.Information("Role {Role} created", role);
    }
}