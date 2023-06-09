using ANY.DuendeIDS.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ANY.Identity.Infrastructure.Common.Models;

public static class IdentityResultExtensions
{
    public static Result ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(result.Errors.Select(e => e.Description));
    }
    //return for Create and Update | Post and Patch 
    public static Result ToApplicationResult(this IdentityResult result, ApplicationUser user, string roles)
    {
        return result.Succeeded
            ? Result.Success(user, roles)
            : Result.Failure(result.Errors.Select(e => e.Description));
    }
}