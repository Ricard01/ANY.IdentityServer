using System.Reflection;
using ANY.Authorization.Tools.PolicyAuthorization;
using ANY.DuendeIDS.Infrastructure.Common.Interfaces;
using ANY.DuendeIDS.Infrastructure.HostingExtensions;
using ANY.DuendeIDS.Infrastructure.Persistence;
using ANY.DuendeIDS.Infrastructure.Repositories.Users;
using ANY.DuendeIDS.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Logging;
using Serilog;

namespace ANY.DuendeIDS.API;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder, IConfiguration configuration)
    {
    
        builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
        builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();
    
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                policy =>
                {
                    policy
                        .WithOrigins("https://localhost:44473")
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });

        if (builder.Environment.IsDevelopment())
        {
            // DbContextConfiguration IRedirectValidator 
            builder.Services.DevelopmentConfiguration(configuration);
        }
        else
        {
            builder.Services.DbContextConfiguration();
        }

    
        builder.Services.IdentityConfiguration();

        // builder.Services.AddLocalApiAuthentication();
        // builder.Services.AddAuthentication("token")
        //     .AddJwtBearer("token", options =>
        //     {
        //         options.Authority = "https:localhost:5000";
        //         options.Audience = "identityApi";
        //
        //         options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
        //
        //         // if token does not contain a dot, it is a reference token
        //         // options.ForwardDefaultSelector = Selector.ForwardReferenceToken("introspection");
        //     });
        
       

        builder.Services.AddScoped<IUserRepository, UserRepository>();

        builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
        builder.Services.AddHttpContextAccessor();


        // orden correcto al final falta validar automapper donde va 
        builder.Services.AddControllers();
        builder.Services.AddRazorPages();

        builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(ApplicationDbContext)));


        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseCors("AllowAllOrigins");
        
        app.UseSerilogRequestLogging();


        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            ApplicationDbSeed.EnsureSeedData(app);
            IdentityModelEventSource.ShowPII = true;
        }

        // Ya todo tiene el orden Correcto https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-api-authorization?view=aspnetcore-7.0
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        // app.UseAuthentication();
        app.UseIdentityServer(); // UseIdentityServer includes a call to UseAuthentication, so it’s not necessary to have both.
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}").RequireAuthorization();

        app.MapRazorPages();
        //
        // app.UseEndpoints(endpoints =>
        // {
        //     
        //     endpoints.MapControllers().RequireAuthorization();
        //     endpoints.MapRazorPages().RequireAuthorization();
        // });

        return app;
    }
}