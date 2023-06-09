using ANY.Identity.Infrastructure.Common.Models;
using ANY.Identity.Infrastructure.Repositories.Users.Dtos;

namespace ANY.Identity.Infrastructure.Repositories.Users;

public interface IUserRepository
{
    
    /// <summary>
    /// List of users with roles
    /// </summary>
    /// <returns>List of all the users with roles.</returns>
    Task<UsersVm> Get();

    
    ///<summary> Get a single user by Id </summary>
    ///<remarks> Gets a single user by Id with his specific roles</remarks>
    ///<returns>Single user with roles</returns>
    Task<UserDto> Get(Guid userId);


    /// <summary>
    /// Creates the user with roles
    /// </summary>
    /// <returns>The result from and identityResult operation </returns>
    Task<Result> CreateAsync(User userRequest);

    
    /// <summary>
    /// Updates the user information with role(s)
    /// </summary>
    /// <returns>The result from and identityResult operation </returns>
    Task<Result> UpdateAsync(Guid userId, User userRequest);

    
    /// <summary>
    /// Deletes the user by id
    /// </summary>
    /// <returns>The result from and identityResult operation </returns>
    Task<Result> DeleteAsync(Guid userId);
    
}