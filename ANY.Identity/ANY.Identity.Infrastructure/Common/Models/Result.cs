using ANY.DuendeIDS.Domain.Entities;

namespace ANY.Identity.Infrastructure.Common.Models;

public class Result
{
    private Result(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }

    //return for Create and Update | Post and PAtch 

    public Guid? UserId { get; set; }
    public string? Role { get; set; }
    
    public string? UserName { get; set; }

    public bool Succeeded { get; init; }

    public string[] Errors { get; init; }

    public static Result Success()
    {
        return new Result(true, Array.Empty<string>());
    }

    public static Result Failure(IEnumerable<string> errors)
    {
        return new Result(false, errors);
    }

    //return for Create and Update | Post and PAtch 
    public static Result Success(ApplicationUser user, string rol)
    {
        return new Result(true, new string[] { }) { UserId = user.Id, UserName = user.UserName, Role = rol };
    }
}