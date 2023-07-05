using System.Text.Json.Serialization;
using ANY.Authorization.Tools;
using ANY.Authorization.Tools.PolicyAuthorization;
using ANY.DuendeIDS.Infrastructure.Common.Models;
using ANY.DuendeIDS.Infrastructure.Repositories.Users;
using ANY.DuendeIDS.Infrastructure.Repositories.Users.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ANY.DuendeIDS.API.Controllers;

public class UserInfo
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("claims")] public IEnumerable<string> Claims { get; set; }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("Claims")]
    [AllowAnonymous]
    public async Task<JsonResult> GetClaims()
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        claims.Add(new { Type = "Bearer", Value = token }!);


        return new JsonResult(claims);
    }

    [HttpGet("Permissions")]
    public UserInfo GetPermissions()
    {
        var claims1 = User.Claims.Select(c => new { c.Type, c.Value });

        Log.Information("claims: {Claims}", claims1);
        var packedPermissions = HttpContext.User?.Claims
            .SingleOrDefault(x => x.Type == Constants.ClaimTypePermissions);
        var claims = packedPermissions?.Value.UnpackPermissionsFromString().Select(x => x.ToString());
        return new UserInfo()
        {
            Id = "1",
            Claims = claims
        };
    }


    [HttpGet]
    [Requires( Permissions.UserAllAccess)]
    public async Task<ActionResult<UsersVm>> Get()
    {
        var users = await _userRepository.Get();

        return Ok(users);
    }


    [Requires(PermissionOperator.Or, Permissions.UserAllAccess, Permissions.UserRead)]
    [HttpGet("{userId}")]
    public async Task<ActionResult<UserDto>> Get(Guid userId)
    {
        var user = await _userRepository.Get(userId);

        return Ok(user);
    }


    [HttpPost]
    [Requires(PermissionOperator.And, Permissions.UserAllAccess, Permissions.UserCreate)]
    public async Task<ActionResult> Post([FromBody] User userRequest)
    {
        var result = await _userRepository.CreateAsync(userRequest);


        return Ok(result);
    }


    [HttpPatch("{userId}")]
    [Requires(Permissions.OrderCreate)]
    public async Task<IActionResult> Patch(Guid userId, [FromBody] User userRequest)
    {
        var result = await _userRepository.UpdateAsync(userId, userRequest);

        return Ok(result);
    }


    [HttpDelete("{userId}")]
    public async Task<ActionResult> Delete(Guid userId)
    {
        var result = await _userRepository.DeleteAsync(userId);

        return Ok(result);
    }
}