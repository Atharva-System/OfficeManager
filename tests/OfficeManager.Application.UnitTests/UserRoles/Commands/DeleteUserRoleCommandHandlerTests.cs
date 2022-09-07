using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Feature.UserRoles.Commands;
using OfficeManager.Application.UnitTests.Mocks;

namespace OfficeManager.Application.UnitTests.UserRoles.Commands
{
    public class DeleteUserRoleCommandHandlerTests : MockUserRoleContext
    {
        [Fact]
        public async Task When_UserRole_Deleted()
        {
            var handler = new DeleteUserRoleCommandHandler(mockContext.Object);

            var result = await handler.Handle(DeleteUserRoleCommand(), CancellationToken.None);

            result.ShouldBeOfType<Result>();

            result.Message.ShouldBe(Messages.DeletedSuccessfully);

            var userRoleMappings = mockContext.Object.UserRoleMapping;

            userRoleMappings.Count().ShouldBe(0);
        }
    }
}
