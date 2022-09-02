using MockQueryable.Moq;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.UnitTests.Mocks
{
    public class MockDesignationsContext : BaseMockContext
    {
        protected readonly Mock<IApplicationDbContext> mockContext;

        public MockDesignationsContext()
        {
            mockContext = GetDesignationsDbContext();
        }

        protected Mock<IApplicationDbContext> GetDesignationsDbContext()
        {
            var mockContext = new Mock<IApplicationDbContext>();

            //Designation
            mockContext.Setup(r => r.Designation).Returns(GetDesignations().AsQueryable().BuildMockDbSet().Object);

            return mockContext;
        }

        protected List<Designation> GetDesignations()
        {
            return new List<Designation>{
             new Designation{
                 Id = 1,
                 Name = "Software Engineer",
                 Description="Software Engineer",
                 IsActive = true
             },
             new Designation{
                 Id = 1,
                 Name = "QA Engineer",
                 Description="QA Engineer",
                 IsActive = true
             },
         };
        }
    }
}
