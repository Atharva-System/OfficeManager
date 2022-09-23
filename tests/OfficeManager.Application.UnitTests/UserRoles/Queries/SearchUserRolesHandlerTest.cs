using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Feature.UserRoles.Queries;
using OfficeManager.Application.UnitTests.Mocks;
using OfficeManager.Application.Wrappers.Concrete;
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

            result.ShouldBeOfType<DataResponse<PaginatedList<RolesDTO>>>();

            DataResponse<PaginatedList<RolesDTO>> response = (DataResponse<PaginatedList<RolesDTO>>)result;

            response.StatusCode.ShouldBe(StatusCodes.Accepted);

            response.Success.ShouldBe(true);

            response.Message.ShouldBe(Messages.DataFound);

            response.Data.Items.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetRolesListWithQueryParams()
        {
            var result = await handler.Handle(new SearchUserRoles { Search = "Admin" }, CancellationToken.None);

            result.ShouldBeOfType<DataResponse<PaginatedList<RolesDTO>>>();

            DataResponse<PaginatedList<RolesDTO>> response = (DataResponse<PaginatedList<RolesDTO>>)result;

            result.StatusCode.ShouldBe(StatusCodes.Accepted);

            response.Success.ShouldBe(true);

            response.Message.ShouldBe(Messages.DataFound);

            response.Data.Items.Count.ShouldBe(1);
        }

        [Fact]
        public async Task GetRolesListWithQueryParamsAndPagination()
        {
            var result = await handler.Handle(new SearchUserRoles { Search = "Admin", Page_No = 1, Page_Size = 10 }, CancellationToken.None);

            result.ShouldBeOfType<DataResponse<PaginatedList<RolesDTO>>>();

            DataResponse<PaginatedList<RolesDTO>> response = (DataResponse<PaginatedList<RolesDTO>>)result;

            result.StatusCode.ShouldBe(StatusCodes.Accepted);

            response.Success.ShouldBe(true);

            response.Message.ShouldBe(Messages.DataFound);

            response.Data.Items.Count.ShouldBe(1);
        }

        [Fact]
        public async Task GetRolesListWithQueryParamsDataFound()
        {
            var result = await handler.Handle(new SearchUserRoles { Search = "Admin" }, CancellationToken.None);

            result.ShouldBeOfType<DataResponse<PaginatedList<RolesDTO>>>();

            DataResponse<PaginatedList<RolesDTO>> response = (DataResponse<PaginatedList<RolesDTO>>)result;

            result.StatusCode.ShouldBe(StatusCodes.Accepted);

            response.Success.ShouldBe(true);

            response.Message.ShouldBe(Messages.DataFound);

            response.Data.Items.Count.ShouldBe(1);
        }

        //[Fact]
        //public async Task GetAllRolesListExceptionThrown()
        //{
        //    var result = await handler.Handle(new SearchUserRoles(), CancellationToken.None);

        //    result.ShouldBeOfType<ErrorResponse>();

        //    ErrorResponse response = (ErrorResponse)result;

        //    response.StatusCode.ShouldBe(StatusCodes.InternalServerError);

        //    response.Success.ShouldBe(false);

        //    response.Errors.Count.ShouldBeGreaterThan(0);
        //}
    }
}
