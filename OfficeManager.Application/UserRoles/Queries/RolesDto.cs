using AutoMapper;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.ApplicationRoles.Queries
{
    public class RolesDto : IMapFrom<RoleMaster>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
