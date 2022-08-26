using AutoMapper;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.UserRoles.Queries
{
    public class UserRoleDto : IMapFrom<UserRoleMapping>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserRoleMapping, UserRoleDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(x => x.RoleName, opt => opt.MapFrom(s => s.Roles.Name))
                .ForMember(x => x.UserId, opt => opt.MapFrom(s => s.UserId))
                .ForMember(x => x.RoleId, opt => opt.MapFrom(s => s.RoleId));
        }
    }
}
