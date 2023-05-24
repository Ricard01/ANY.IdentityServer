using ANY.DuendeIDS.Infrastructure.HostingConfiguration;
using ANY.DuendeIDS.Infrastructure.Persistence;
using Serilog;

namespace ANY.DuendeIDS.API;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddRazorPages();

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.ConfigDevelopExtension(configuration);
            
            
        }
        else
        {
            builder.Services.ConfigDbContext();
        }

        builder.Services.ConfigIdentityServer();

        builder.Services.AddAuthentication();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            ApplicationDbSeed.EnsureSeedData(app);
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();

        app.MapRazorPages()
            .RequireAuthorization();

        return app;
    }
}