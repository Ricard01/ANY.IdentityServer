using ANY.Authorization.Tools.Constants;
using ANY.Authorization.Tools.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ANY.Identity.API.Controllers;

[ApiController]
[Route("[controller]")]

public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    [RequiresPermission(Permissions.UserAllAccess)]
    // [Authorize("RequireInteractiveUser")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
    
    
    [HttpGet("Permissions")]
    [AllowAnonymous]
    public IEnumerable<Permissions> GetPermissionsEnumerable()
    {
        var claims = HttpContext.User.Claims;
        var permissionsClaim =
            claims?.SingleOrDefault(c => c.Type == PermissionConstants.ClaimType);
        
        return permissionsClaim?.Value.UnpackPermissionsFromString();
    }
}