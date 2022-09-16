using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Feature.UserRoles.Queries;
using OfficeManager.Application.UnitTests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeManager.Application.UnitTests.UserRoles.Queries
{
    public class SearchUserRolesHandlerTest : MockUserRoleContext
    {
        private readonly SearchUserRolesHandler handler;
        public SearchUserRolesHandlerTest()
        {
            handler = new SearchUserRolesHandler(mockContext.Object, mapper);
        }

        [Fact]
        public async Task GetAllRolesList()
        {
            var result = await handler.Handle(new SearchUserRoles(), CancellationToken.None);

            result.ShouldBeOfType<Response<PaginatedList<RolesDTO>>>();

            result.StatusCode.ShouldBe(StausCodes.Accepted);

            result.IsSuccess.ShouldBe(true);

            result.Message.ShouldBe(Messages.DataFound);

            result.Data.Items.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetRolesListWithQueryParams()
        {
            var result = await handler.Handle(new SearchUserRoles { Search = "Admin" }, CancellationToken.None);

            result.ShouldBeOfType<Response<PaginatedList<RolesDTO>>>();

            result.StatusCode.ShouldBe(StausCodes.Accepted);

            result.IsSuccess.ShouldBe(true);

            result.Message.ShouldBe(Messages.DataFound);

            result.Data.Items.Count.ShouldBe(1);
        }

        [Fact]
        public async Task GetRolesListWithQueryParamsAndPagination()
        {
            var result = await handler.Handle(new SearchUserRoles { Search = "Admin", Page_No = 1, Page_Size = 10 }, CancellationToken.None);

            result.ShouldBeOfType<Response<PaginatedList<RolesDTO>>>();

            result.StatusCode.ShouldBe(StausCodes.Accepted);

            result.IsSuccess.ShouldBe(true);

            result.Message.ShouldBe(Messages.DataFound);

            result.Data.Items.Count.ShouldBe(1);
        }

        [Fact]
        public async Task GetRolesListWithQueryParamsDataFound()
        {
            var result = await handler.Handle(new SearchUserRoles { Search = "Admin" }, CancellationToken.None);

            result.ShouldBeOfType<Response<PaginatedList<RolesDTO>>>();

            result.StatusCode.ShouldBe(StausCodes.Accepted);

            result.IsSuccess.ShouldBe(true);

            result.Message.ShouldBe(Messages.DataFound);

            result.Data.Items.Count.ShouldBe(1);
        }

        [Fact]
        public async Task GetAllRolesListExceptionThrown()
        {
            var RoleMockSet = new Mock<DbSet<RoleMaster>>();
            mockContext.Setup(r => r.Roles).Returns(RoleMockSet.Object);

            var result = await handler.Handle(new SearchUserRoles(), CancellationToken.None);

            result.ShouldBeOfType<Response<PaginatedList<RolesDTO>>>();

            result.StatusCode.ShouldBe(StausCodes.InternalServerError);

            result.IsSuccess.ShouldBe(false);

            result.Message.ShouldBe(Messages.IssueWithData);

            result.Errors.Count.ShouldBeGreaterThan(0);

            result.Data.ShouldBeNull();
        }
    }
}
