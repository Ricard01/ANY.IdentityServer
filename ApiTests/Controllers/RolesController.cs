using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiTests.Controllers;

[ApiController]
[Route("api2/[controller]")]
public class RolesController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<JsonResult> Get()
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        claims.Add(new { Type = "Bearer", Value = token }!);

        
        
        return new JsonResult(claims);
    }
}