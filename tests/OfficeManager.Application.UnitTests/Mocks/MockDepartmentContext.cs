using MockQueryable.Moq;
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

            //Department
            mockContext.Setup(r => r.Department).Returns(GetDepartments().AsQueryable().BuildMockDbSet().Object);

            return mockContext;
        }

        protected List<Department> GetDepartments()
        {
            return new List<Department>{
             new Department{
                 Id = 1,
                 Name = ".Net",
                 Description=".Net",
                 IsActive = true
             },
             new Department{
                 Id = 1,
                 Name = "Analytics",
                 Description="Analytics",
                 IsActive = true
             },
         };
        }
    }
}
