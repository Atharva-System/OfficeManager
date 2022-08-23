using OfficeManager.Application.ApplicationRoles.Commands.CreateApplicationUserRoles;
using OfficeManager.Application.ApplicationRoles.Commands.DeleteUserRoles;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.UnitTests.Mocks
{
    public class MockUserRoleContext : BaseMockContext
    {
        protected readonly Mock<IApplicationDbContext> _mockContext;

        public MockUserRoleContext()
        {
            _mockContext = GetUserRoleDbContext();
        }
        public Mock<IApplicationDbContext> GetUserRoleDbContext()
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

            mockContext.Setup(r => r.UserRoleMapping.Add(It.IsAny<UserRoleMapping>())).Callback((UserRoleMapping userRoleMapping) =>
            {
                userRoleMappingLists.Add(userRoleMapping);
            });

            mockContext.Setup(r => r.UserRoleMapping.Remove(It.IsAny<UserRoleMapping>())).Callback((UserRoleMapping userRoleMapping) =>
            {
                userRoleMappingLists.Remove(userRoleMapping);
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

        protected CreateUserRolesCommand AddUserRoleCommand()
        {
            return new CreateUserRolesCommand
            {
                UserId = 1,
                RoleId = 2,
            };
        }

        protected DeleteUserRoleCommand DeleteUserRoleCommand()
        {
            return new DeleteUserRoleCommand(1);
        }
    }
}
