using ANY.Identity.Infrastructure.Common.Models;
using ANY.Identity.Infrastructure.Repositories.Users;
using ANY.Identity.Infrastructure.Repositories.Users.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace ANY.Identity.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    [HttpGet]
    public async Task<ActionResult<UsersVm>> Get()
    {
        var users = await _userRepository.Get();

        return Ok(users);
    }


    [HttpGet("{userId}")]
    public async Task<ActionResult<UserDto>> Get(Guid userId)
    {
        var user = await _userRepository.Get(userId);

        return Ok(user);
    }


    [HttpPost]
    public async Task<ActionResult> Post([FromBody] User userRequest)
    {
        var result = await _userRepository.CreateAsync(userRequest);
       

        return Ok(result);
    }


    [HttpPatch("{userId}")]
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