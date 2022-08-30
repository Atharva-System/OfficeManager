using OfficeManager.Application.ApplicationRoles.Queries;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.UnitTests.Mocks;

namespace OfficeManager.Application.UnitTests.UserRoles.Queries
{
    public class GetUserRolesQueryHandlerTests : MockUserRoleContext
    {
        [Fact]
        public async Task GetUserRolesListTest()
        {
            var handler = new GetUserRolesQueryHandler(mockContext.Object, mapper);

            var result = await handler.Handle(new GetUserRoles(), CancellationToken.None);

            result.ShouldBeOfType<List<RolesDTO>>();

            result.Count.ShouldBe(2);
        }
    }
}
