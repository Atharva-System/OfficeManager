using AutoMapper;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Dtos
{
    public class LoggedInUserDTO : IMapFrom<UserMaster>
    {
        public int UserId { get; set; }
        public int EmployeeNo { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public List<UserRoleDTO> Roles { get; set; } = new List<UserRoleDTO>();
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserMaster, LoggedInUserDTO>()
                .ForMember(x => x.UserId, opt => opt.MapFrom(s => s.Id))
                .ForMember(x => x.EmployeeNo, opt => opt.MapFrom(s => s.Employee.EmployeeNo))
                .ForMember(x => x.Email, opt => opt.MapFrom(s => s.Email));
        }
    }
}
