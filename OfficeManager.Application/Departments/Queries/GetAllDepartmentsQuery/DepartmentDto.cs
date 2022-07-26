using AutoMapper;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Departments.Queries.GetAllDepartmentsQuery
{
    public class DepartmentDto : IMapFrom<DepartmentMaster>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<DepartmentMaster, DepartmentDto>();
        }
    }
}
