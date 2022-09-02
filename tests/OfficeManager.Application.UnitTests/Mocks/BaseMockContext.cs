using AutoMapper;
using OfficeManager.Application.Common.Mappings;

namespace OfficeManager.Application.UnitTests.Mocks
{
    public class BaseMockContext
    {
        protected readonly IMapper mapper;
        public BaseMockContext()
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });

            mapper = mapperConfig.CreateMapper();
        }
    }
}
