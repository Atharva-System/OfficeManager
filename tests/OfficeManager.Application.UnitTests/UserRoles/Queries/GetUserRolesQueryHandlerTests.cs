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
            var handler = new GetUserRolesQueryHandler(_mockContext.Object, _mapper);

            var result = await handler.Handle(new GetUserRolesQuery(), CancellationToken.None);

            result.ShouldBeOfType<List<RolesDTO>>();

            result.Count.ShouldBe(2);
        }
    }
}
