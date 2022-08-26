using OfficeManager.Application.Common.Mappings;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Designations.Queries.SearchDesignations
{
    public class DesignationDto : IMapFrom<Designation>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
