using AutoMapper;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Departments.Queries.SearchDepartments
{
    public class DepartmentDto : IMapFrom<DepartMent>
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
