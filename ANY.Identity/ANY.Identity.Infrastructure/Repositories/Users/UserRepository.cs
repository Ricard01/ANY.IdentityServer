using ANY.DuendeIDS.Domain.Entities;
using ANY.Identity.Infrastructure.Common.Exceptions;
using ANY.Identity.Infrastructure.Common.Interfaces;
using ANY.Identity.Infrastructure.Common.Models;
using ANY.Identity.Infrastructure.Repositories.Users.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ANY.Identity.Infrastructure.Repositories.Users;

public class UserRepository : IUserRepository
{
     private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public UserRepository(ICurrentUserService currentUserService,UserManager<ApplicationUser> userManager, IMapper mapper)
    {
       
         _currentUserService = currentUserService;
        _userManager = userManager;
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
        var user = new ApplicationUser
        {
            UserName = userRequest.UserName,
            Name = userRequest.Name, 
            // Nombre = userRequest.Nombre,
            // ApellidoPaterno = userRequest.ApellidoPaterno,
            Email = userRequest.Email,

            //CreatedAt = DateTime.Now
        };

        var result = await _userManager.CreateAsync(user, userRequest.Password);


        return result.ToApplicationResult();
    }


    public async Task<Result> UpdateAsync(Guid userId, User userRequest)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user == null)
        {
            throw new NotFoundException(nameof(ApplicationUser), userId);
        }

        if (userRequest.Name != null) user.Name = userRequest.Name;
        // user.ApellidoPaterno = userRequest.ApellidoPaterno;
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
        if ( new Guid(_currentUserService.UserId) == userId)
        {
            return IdentityResult.Failed(new IdentityError { Description = $"Can't delete the user that you are currently logged in" }).ToApplicationResult(); ;
        }
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