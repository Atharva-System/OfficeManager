using AutoMapper;
using Microsoft.AspNetCore.Identity;
using OfficeManager.Application.ApplicationRoles.Queries;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Application.UserRoles.Queries;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.ApplicationUsers.Commands.LoginApplicationUser
{
    public class LoggedInUserDto : IMapFrom<UserMaster>
    {
        public int UserId { get; set; }
        public int EmployeeNo { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public List<UserRoleDto> Roles { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserMaster, LoggedInUserDto>()
                .ForMember(x => x.UserId, opt => opt.MapFrom(s => s.Id))
                .ForMember(x => x.EmployeeNo, opt => opt.MapFrom(s => s.Employee.EmployeeNo))
                .ForMember(x => x.Email, opt => opt.MapFrom(s => s.Email));
        }
    }
}
