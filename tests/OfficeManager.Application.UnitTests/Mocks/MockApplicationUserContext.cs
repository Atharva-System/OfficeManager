﻿using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Dtos;

namespace OfficeManager.Application.UnitTests.Mocks
{
    public class MockApplicationUserContext : BaseMockContext
    {
        protected readonly Mock<IApplicationDbContext> mockContext;
        protected readonly Mock<ICurrentUserServices> currentUserService;
        protected readonly Mock<ITokenService> tokenService;
        public MockApplicationUserContext()
        {
            currentUserService = new Mock<ICurrentUserServices>();
            currentUserService.Setup(x => x.loggedInUser).Returns(new Mock<LoggedInUserDTO>().Object);
            mockContext = GetApplicationUserDbContext();
            tokenService = new Mock<ITokenService>(); tokenService = new Mock<ITokenService>();
            tokenService.Setup(t => t.CreateToken(It.IsAny<LoggedInUserDTO>()))
                    .Returns(new TokenDTO
                    {
                        AccessToken = Guid.NewGuid().ToString(),
                        RefreshToken = Guid.NewGuid().ToString(),
                        AccessTokenExpiration = DateTime.Now.AddMinutes(10),
                        RefreshTokenExpiration = DateTime.Now.AddMinutes(10),
                    });
        }
        protected Mock<IApplicationDbContext> GetApplicationUserDbContext()
        {
            var mockContext = new Mock<IApplicationDbContext>();

            //Users
            var userList = GetUsers();
            mockContext.Setup(r => r.Users).Returns(userList.AsQueryable().BuildMockDbSet().Object);
            mockContext.Setup(m => m.Users.AddAsync(It.IsAny<UserMaster>(), default)).Callback<UserMaster, CancellationToken>((s, token) =>
            {
                userList.Add(s);
            });

            // User Role Mapping
            var userRoleMappingLists = GetUserRoleMappings();
            mockContext.Setup(r => r.UserRoleMapping).Returns(userRoleMappingLists.AsQueryable().BuildMockDbSet().Object);
            mockContext.Setup(m => m.UserRoleMapping.AddAsync(It.IsAny<UserRoleMapping>(), default)).Callback<UserRoleMapping, CancellationToken>((s, token) =>
            {
                userRoleMappingLists.Add(s);
            });

            //Employees
            var employeesMappingLists = new List<Employee>();
            mockContext.Setup(r => r.Employees).Returns(employeesMappingLists.AsQueryable().BuildMockDbSet().Object);
            mockContext.Setup(m => m.Employees.AddAsync(It.IsAny<Employee>(), default)).Callback<Employee, CancellationToken>((s, token) =>
            {
                employeesMappingLists.Add(s);
            });

            var refreshTokenMockSet = new Mock<DbSet<RefreshToken>>();
            mockContext.Setup(r => r.RefreshToken).Returns(refreshTokenMockSet.Object);


            return mockContext;
        }

        protected List<UserMaster> GetUsers()
        {
            return new List<UserMaster>{
             new UserMaster{
                 Id = 1,
                 Email = "test@tes.com",
                 PasswordHash = "$2b$10$AYslpSQhpac4CCjg4OJ00OY/DZ8UFeWQH9gBQnop9jTias0puE5Ja",
                 IsActive = true,
                 Employee = new Employee{
                 EmployeeNo = 99999,
                 DesignationId =1,
                 DepartmentId = 1,
                 Email ="test@tes.com",
                 DateOfBirth = DateTime.Now,
                 DateOfJoining = DateTime.Now,
                 }
             }
         };
        }

        protected List<UserRoleMapping> GetUserRoleMappings()
        {
            return new List<UserRoleMapping>{
             new UserRoleMapping{
                 Id = 1,
                 UserId = 1,
                 RoleId = 1,
                 Roles = new RoleMaster{
                 Id = 1,
                 Name = "Admin",
                 Description = "Admin Role",
                 IsActive = true
             }
             }
         };
        }
    }
}
