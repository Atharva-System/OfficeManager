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
            var depart = GetDepartments();
            mockContext.Setup(r => r.Department).Returns(depart.AsQueryable().BuildMockDbSet().Object);
            mockContext.Setup(m => m.Department.AddAsync(It.IsAny<Department>(), default)).Callback<Department, CancellationToken>((s, token) =>
            {
                depart.Add(s);
            });
            mockContext.Setup(m => m.Department.Remove(It.IsAny<Department>())).Callback<Department>(s =>
            {
                depart.Remove(s);
            });
            mockContext.Setup(r => r.Department).Returns(GetDepartments().AsQueryable().BuildMockDbSet().Object);
         
            mockContext.Object.SaveChangesAsync();
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
                 Id = 2,
                 Name = "Analytics",
                 Description="Analytics",
                 IsActive = true
             },
         };
        }
    }
}