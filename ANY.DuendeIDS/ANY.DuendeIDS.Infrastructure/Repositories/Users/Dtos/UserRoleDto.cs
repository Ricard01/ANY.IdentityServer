using ANY.DuendeIDS.Domain.Entities;
using AutoMapper;


namespace ANY.DuendeIDS.Infrastructure.Repositories.Users.Dtos;

[AutoMap(typeof(ApplicationUserRole))]
public class UserRoleDto 
{
   
    public string? RoleName { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ApplicationUserRole, UserRoleDto>()
            .ForMember(dto => dto.RoleName, opt => opt.MapFrom(ur => ur.Role.Name ));

    }
}