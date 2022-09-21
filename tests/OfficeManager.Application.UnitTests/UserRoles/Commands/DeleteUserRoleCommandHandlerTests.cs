using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Feature.UserRoles.Commands;
using OfficeManager.Application.UnitTests.Mocks;
using OfficeManager.Application.Wrappers.Concrete;

namespace OfficeManager.Application.UnitTests.UserRoles.Commands
{
    public class DeleteUserRoleCommandHandlerTests : MockUserRoleContext
    {
        [Fact]
        public async Task When_UserRole_Deleted()
        {
            var handler = new DeleteUserRoleCommandHandler(mockContext.Object);

            var result = await handler.Handle(DeleteUserRoleCommand(), CancellationToken.None);

            result.ShouldBeOfType<SuccessResponse>();

            SuccessResponse response = (SuccessResponse)result;

            response.Message.ShouldBe(Messages.DeletedSuccessfully);

            var userRoleMappings = mockContext.Object.UserRoleMapping;

            userRoleMappings.Count().ShouldBe(0);
        }
    }
}
