// using ANY.DuendeIDS.Domain.Entities;
// using ANY.Identity.Infrastructure.Repositories.Users.Dtos;
// using AutoMapper;
//
// namespace ANY.Identity.Infrastructure.Common.Mappings;
//
// public class AutoMapProfile : Profile
// {
//     public AutoMapProfile()
//     {
//         string? userId = null;
//         CreateMap<ApplicationUserRole, UserDto>()
//             .ForMember(dto => dto.Id, opt 
//                 => opt.MapFrom(ur => userId));
//
//
//         CreateMap<ApplicationUserRole, UserRoleDto>()
//             .ForMember(ur => ur.RoleName, opt
//                 => opt.MapFrom(src => src.Role.Name));
//     }
// }