using OfficeManager.Application.ApplicationRoles.Queries;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.UnitTests.Mocks;
using OfficeManager.Application.Wrappers.Concrete;

namespace OfficeManager.Application.UnitTests.UserRoles.Queries
{
    public class GetUserRolesQueryHandlerTests : MockUserRoleContext
    {
       
        [Fact]
        public async Task GetUserRolesListTest()
        {
            var handler = new GetUserRolesQueryHandler(mockContext.Object, mapper);

            var result = await handler.Handle(new GetUserRoles(), CancellationToken.None);

            result.ShouldBeOfType<DataResponse<List<RolesDTO>>>();

            DataResponse<List<RolesDTO>> response = (DataResponse<List<RolesDTO>>)result;

            response.Data.Count.ShouldBe(2);
        }
    }
}
