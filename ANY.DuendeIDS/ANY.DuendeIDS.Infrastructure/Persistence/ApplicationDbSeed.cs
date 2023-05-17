using ANY.DuendeIDS.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ANY.DuendeIDS.Infrastructure.Persistence;

public class ApplicationDbSeed
{
    public static void EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            // try
            // {
            //     context?.Database.MigrateAsync();
            // }
            // catch (Exception ex)
            // {
            //     Log.Error(ex, "Ocurrio un error al inicializar ApplicationDb");
            //     throw;
            // }

            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var admin = userMgr.FindByNameAsync("admin").Result;

            if (admin != null) return;
            
            admin = new ApplicationUser
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

            Log.Debug("admin created");
        }
    }
}