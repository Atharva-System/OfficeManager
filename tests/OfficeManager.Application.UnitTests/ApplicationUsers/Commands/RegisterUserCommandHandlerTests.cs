using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Feature.ApplicationUsers.Commands;
using OfficeManager.Application.UnitTests.Mocks;

namespace OfficeManager.Application.UnitTests.ApplicationUsers.Commands
{
    public class RegisterUserCommandHandlerTests : MockApplicationUserContext
    {
        private readonly RegisterUserCommandHandler handler;
        public RegisterUserCommandHandlerTests()
        {
            handler = new RegisterUserCommandHandler(mockContext.Object);
        }

        [Fact]
        public async Task User_Registered_Success()
        {
            var result = await handler.Handle(new RegisterUser { EmployeeNo = 1, Password = "Test",DesignationId=1,DepartmentId=1 }, CancellationToken.None);

            result.ShouldBeOfType<Result>();

            result.Succeeded.ShouldBe(true);

            result.Message.ShouldBe(Messages.RegisteredSuccesfully);

            mockContext.Object.Employees.Count().ShouldBe(1);

            mockContext.Object.Users.Count().ShouldBe(2);

            mockContext.Object.UserRoleMapping.Count().ShouldBe(2);
        }

        [Fact]
        public async Task User_Registered_ExceptionThrown()
        {
            var handler = new RegisterUserCommandHandler(new Mock<IApplicationDbContext>().Object);

            var result = await handler.Handle(new RegisterUser { EmployeeNo = 1, Password = "Test", DesignationId = 1, DepartmentId = 1 }, CancellationToken.None);

            result.ShouldBeOfType<Result>();

            result.Succeeded.ShouldBe(false);

            result.Message.ShouldNotBeNull();
        }
    }
}
