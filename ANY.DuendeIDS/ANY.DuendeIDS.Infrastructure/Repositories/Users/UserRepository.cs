using ANY.DuendeIDS.Domain.Entities;
using ANY.DuendeIDS.Infrastructure.Common.Exceptions;
using ANY.DuendeIDS.Infrastructure.Common.Models;
using ANY.DuendeIDS.Infrastructure.Repositories.Users.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ANY.DuendeIDS.Infrastructure.Repositories.Users;

public class UserRepository : IUserRepository
{
    // private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    private readonly IMapper _mapper;

    public UserRepository( UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager, IMapper mapper) //ICurrentUserService currentUserService,
    {
        // _currentUserService = currentUserService;
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
    }


    public async Task<UsersVm> Get()
    {
        return new UsersVm
        {
            Users = await _userManager.Users
                .AsNoTracking()
                .OrderBy(u => u.UserName)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .ToListAsync()
        };
    }


    public async Task<UserDto> Get(Guid userId)
    {
        var user = await _userManager.Users
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new NotFoundException(nameof(ApplicationUser), userId);
        }

        return user;
    }


    public async Task<Result> CreateAsync(User userRequest)
    {
        var roleExists = await _roleManager.RoleExistsAsync(userRequest.Role.Name);

        if (!roleExists)
        {
            return IdentityResult.Failed(new IdentityError
                    { Description = $"Role {userRequest.Role.Name} doesn't exist" })
                .ToApplicationResult();
        }

        var user = new ApplicationUser
        {
            UserName = userRequest.UserName,
            Name = userRequest.Name,
            Email = userRequest.Email,
            //CreatedAt = DateTime.Now
        };

        var result = await _userManager.CreateAsync(user, userRequest.Password);

        if (result.Succeeded)
        {
            // doest return error if roleName doesnt exists so i made the first validation roleExists
            await _userManager.AddToRoleAsync(user, userRequest.Role.Name);

            return result.ToApplicationResult(user, userRequest.Role.Name);
        }

        // if something when wrong creating the user
        return result.ToApplicationResult();
    }


    public async Task<Result> UpdateAsync(Guid userId, User userRequest)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user == null)
        {
            throw new NotFoundException(nameof(ApplicationUser), userId);
        }

        user.Name = userRequest.Name;
        user.Email = userRequest.Email;

        var result = await _userManager.UpdateAsync(user);

        return result.ToApplicationResult();
        //if (result.Succeeded)
        //{

        //    result = await UpdateRolesAsync(user, userRequest.UserRoles);
        //}
    }


    public async Task<Result> DeleteAsync(Guid userId)
    {
        // if (new Guid(_currentUserService.UserId) == userId)
        // {
        //     return IdentityResult.Failed(new IdentityError
        //         { Description = $"Can't delete the user that you are currently logged in" }).ToApplicationResult();
        //     ;
        // }

        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user == null)
        {
            var result = IdentityResult.Failed(new IdentityError { Description = $"User with id {userId} not Found" });
            return result.ToApplicationResult();
        }

        return await DeleteAsync(user);
    }


    private async Task<Result> DeleteAsync(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }
}