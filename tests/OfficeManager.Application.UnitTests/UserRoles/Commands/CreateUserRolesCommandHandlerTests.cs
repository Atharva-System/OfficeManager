using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Feature.UserRoles.Commands;
using OfficeManager.Application.UnitTests.Mocks;

namespace OfficeManager.Application.UnitTests.UserRoles.Commands
{
    public class CreateUserRolesCommandHandlerTests : MockUserRoleContext
    {
        private readonly CreateUserRolesCommandHandler handler;
        public CreateUserRolesCommandHandlerTests()
        {
            handler = new CreateUserRolesCommandHandler(mockContext.Object);
        }

        [Fact]
        public async Task WhenValidUserRoleAdded()
        {
            var result = await handler.Handle(AddUserRoleCommand(), CancellationToken.None);

            result.ShouldBeOfType<Result>();

            result.Message.ShouldBe(Messages.AddedSuccesfully);

            var userRoleMappings = mockContext.Object.UserRoleMapping;

            userRoleMappings.Count().ShouldBe(2);
        }
    }
}
