namespace ANY.DuendeIDS.Infrastructure.Common.Models;

public class User
{
    public User(Role role)
    {
        Role = role;
    }

    public string? UserName { get; set; }

    public string Name { get; set; } = null!;

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public Role Role { get; set; }
}

public class Role
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
}