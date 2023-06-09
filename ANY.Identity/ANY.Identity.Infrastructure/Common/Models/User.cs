namespace ANY.Identity.Infrastructure.Common.Models;

public class User
{
    public string? UserName { get; set; } 
    
     public string? Name { get; set; } 
    //
    // public string? ApellidoPaterno { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }// = null!;
}