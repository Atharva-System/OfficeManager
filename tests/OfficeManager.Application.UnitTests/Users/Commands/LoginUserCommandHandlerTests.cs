using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Feature.Users.Commands;
using OfficeManager.Application.UnitTests.Mocks;
using OfficeManager.Application.Wrappers.Concrete;

namespace OfficeManager.Application.UnitTests.Users.Commands
{
    public class LoginUserCommandHandlerTests : MockApplicationUserContext
    {
        private readonly LoginUserCommandHandler handler;

        public LoginUserCommandHandlerTests()
        {
            handler = new LoginUserCommandHandler(mockContext.Object, mapper, currentUserService.Object, tokenService.Object);
            //handler = new LoginUserCommandHandler(mockContext.Object, mapper, currentUserService.Object);
        }

        [Fact]
        public async Task User_LoggedIn_Success()
        {
            var result = await handler.Handle(new LoginUser { EmployeeNo = "99999", Password = "Atharva@123" }, CancellationToken.None);

            result.ShouldBeOfType<DataResponse<TokenDTO>>();

            DataResponse<TokenDTO> response = (DataResponse<TokenDTO>)result;

            response.StatusCode.ShouldBe(StatusCodes.Accepted);

            response.Success.ShouldBe(true);

            response.Message.ShouldBe(Messages.Success);
        }

        [Fact]
        public async Task User_LoggedIn_Fail()
        {
            var result = await handler.Handle(new LoginUser { EmployeeNo = "99999", Password = "Admin@123" }, CancellationToken.None);

            result.ShouldBeOfType<ErrorResponse>();

            ErrorResponse response = (ErrorResponse)result;

            response.StatusCode.ShouldBe(StatusCodes.BadRequest);

            response.Success.ShouldBe(false);

            response.Errors.Count.ShouldBeGreaterThan(0);
        }

        //[Fact]
        //public async Task User_LoggIn_ExceptionThrown()
        //{
        //    var userMappingMockSet = new Mock<DbSet<UserMaster>>();
        //    mockContext.Setup(r => r.Users).Returns(userMappingMockSet.Object);

        //    var result = await handler.Handle(new LoginUser { EmployeeNo = 99999, Password = "Admin@123" }, CancellationToken.None);

        //    result.ShouldBeOfType<Response<LoggedInUserDTO>>();

        //    result.StatusCode.ShouldBe(StausCodes.InternalServerError);

        //    result.IsSuccess.ShouldBe(false);

        //    result.Message.ShouldBe(Messages.IssueWithData);

        //    result.Errors.Count.ShouldBeGreaterThan(0);
        //}
    }
}
