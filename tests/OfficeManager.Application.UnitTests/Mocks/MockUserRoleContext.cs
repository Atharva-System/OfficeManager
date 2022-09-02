using MockQueryable.Moq;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Feature.UserRoles.Commands;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.UnitTests.Mocks
{
    public class MockUserRoleContext : BaseMockContext
    {
        protected readonly Mock<IApplicationDbContext> mockContext;

        public MockUserRoleContext()
        {
            mockContext = GetUserRoleDbContext();
        }
        protected Mock<IApplicationDbContext> GetUserRoleDbContext()
        {
            var mockContext = new Mock<IApplicationDbContext>();

            //User Roles
            mockContext.Setup(r => r.Roles).Returns(GetUserRoles().AsQueryable().BuildMockDbSet().Object);

            // User Role Mapping
            var userRoleMappingLists = GetUserRoleMappings();

            // This is used to inititate object
            //var userRoleMappingMockSet = new Mock<DbSet<UserRoleMapping>>();
            //mockContext.Setup(r => r.UserRoleMapping).Returns(userRoleMappingMockSet.Object);

            mockContext.Setup(r => r.UserRoleMapping).Returns(userRoleMappingLists.AsQueryable().BuildMockDbSet().Object);
            mockContext.Setup(m => m.UserRoleMapping.AddAsync(It.IsAny<UserRoleMapping>(), default)).Callback<UserRoleMapping, CancellationToken>((s, token) =>
            {
                userRoleMappingLists.Add(s);
            });
            mockContext.Setup(m => m.UserRoleMapping.Remove(It.IsAny<UserRoleMapping>())).Callback<UserRoleMapping>(s =>
            {
                userRoleMappingLists.Remove(s);
            });
            

            return mockContext;
        }

        protected List<RoleMaster> GetUserRoles()
        {
            return new List<RoleMaster>{
             new RoleMaster{
                 Id = 1,
                 Name = "Admin",
                 Description="Admin Role",
                 IsActive = true
             },
             new RoleMaster{
                 Id = 1,
                 Name = "User",
                 Description="User Role",
                 IsActive = true
             },
         };
        }

        protected List<UserRoleMapping> GetUserRoleMappings()
        {
            return new List<UserRoleMapping>{
             new UserRoleMapping{
                 Id = 1,
                 UserId = 1,
                 RoleId = 1
             }
         };
        }

        protected CreateUserRoles AddUserRoleCommand()
        {
            return new CreateUserRoles
            {
                UserId = 1,
                RoleId = 2,
            };
        }

        protected DeleteUserRole DeleteUserRoleCommand()
        {
            return new DeleteUserRole(1);
        }
    }
}
