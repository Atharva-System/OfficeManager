using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.UnitTests.Mocks
{
    public class MockDepartmentContext : BaseMockContext
    {
        protected readonly Mock<IApplicationDbContext> mockContext;

        public MockDepartmentContext()
        {
            mockContext = GetDepartmentDbContext();
        }

        protected Mock<IApplicationDbContext> GetDepartmentDbContext()
        {
            var mockContext = new Mock<IApplicationDbContext>();

            //DepartMent
            mockContext.Setup(r => r.DepartMent).Returns(GetDepartments().AsQueryable().BuildMockDbSet().Object);

            return mockContext;
        }

        protected List<DepartMent> GetDepartments()
        {
            return new List<DepartMent>{
             new DepartMent{
                 Id = 1,
                 Name = ".Net",
                 Description=".Net",
                 IsActive = true
             },
             new DepartMent{
                 Id = 1,
                 Name = "Analytics",
                 Description="Analytics",
                 IsActive = true
             },
         };
        }
    }
}
