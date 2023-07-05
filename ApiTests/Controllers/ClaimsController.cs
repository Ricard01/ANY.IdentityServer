using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ApiTests.Controllers;

[ApiController]
[Route("api2/[controller]")]
public class ClaimsController : ControllerBase
{
    
    [HttpGet]
    public async Task<JsonResult> Get()
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        claims.Add(new { Type = "Bearer", Value = token }!);

        
        
        return new JsonResult(claims);
    }
    
    
}