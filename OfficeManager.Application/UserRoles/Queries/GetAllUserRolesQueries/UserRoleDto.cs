using AutoMapper;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.UserRoles.Queries.GetAllUserRolesQueries
{
    public class UserRoleDto : IMapFrom<UserRole>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserRole, UserRoleDto>();
        }
    }
}
