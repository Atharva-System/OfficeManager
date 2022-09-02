using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Feature.ApplicationUsers.Commands;
using OfficeManager.Application.UnitTests.Mocks;

namespace OfficeManager.Application.UnitTests.ApplicationUsers.Commands
{
    public class LoginUserCommandHandlerTests : MockApplicationUserContext
    {
        private readonly LoginUserCommandHandler handler;
        public LoginUserCommandHandlerTests()
        {
            
            handler = new LoginUserCommandHandler(mockContext.Object, mapper, currentUserService.Object);
        }

        [Fact]
        public async Task User_LoggedIn_Success()
        {
            var result = await handler.Handle(new LoginUser { EmployeeNo = 99999 , Password = "Atharva@123"}, CancellationToken.None);

            result.ShouldBeOfType<Response<LoggedInUserDTO>>();

            result.StatusCode.ShouldBe(StausCodes.Accepted);

            result.IsSuccess.ShouldBe(true);

            result.Message.ShouldBe(Messages.Success);
        }

        [Fact]
        public async Task User_LoggedIn_Fail()
        {
            var result = await handler.Handle(new LoginUser { EmployeeNo = 99999, Password = "Admin@123" }, CancellationToken.None);

            result.ShouldBeOfType<Response<LoggedInUserDTO>>();

            result.StatusCode.ShouldBe(StausCodes.BadRequest);

            result.IsSuccess.ShouldBe(false);

            result.Message.ShouldBe(Messages.CheckCredentials);
        }

        [Fact]
        public async Task User_LoggIn_ExceptionThrown()
        {
            var userMappingMockSet = new Mock<DbSet<UserMaster>>();
            mockContext.Setup(r => r.Users).Returns(userMappingMockSet.Object);

            var result = await handler.Handle(new LoginUser { EmployeeNo = 99999, Password = "Admin@123" }, CancellationToken.None);

            result.ShouldBeOfType<Response<LoggedInUserDTO>>();

            result.StatusCode.ShouldBe(StausCodes.InternalServerError);

            result.IsSuccess.ShouldBe(false);

            result.Message.ShouldBe(Messages.IssueWithData);

            result.Errors.Count.ShouldBeGreaterThan(0);
        }
    }
}
