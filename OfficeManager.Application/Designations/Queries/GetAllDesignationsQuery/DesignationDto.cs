using AutoMapper;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Designations.Queries.GetAllDesignationsQuery
{
    public class DesignationDto : IMapFrom<DesignationMaster>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<DesignationMaster, DesignationDto>();
        }
    }
}
