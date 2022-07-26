﻿using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.UnitTests.Mocks;
using OfficeManager.Application.Feature.Designations.Queries;
using OfficeManager.Application.Wrappers.Concrete;

namespace OfficeManager.Application.UnitTests.Designations.Queries
{
    public class SearchDesignationsQueryHandlerTests : MockDesignationsContext
    {
        private readonly SearchDesginatoinsHandler handler;
        public SearchDesignationsQueryHandlerTests()
        {
            handler = new SearchDesginatoinsHandler(mockContext.Object, mapper);
        }

        [Fact]
        public async Task GetAllDesignationList()
        {
            var result = await handler.Handle(new SearchDesignations(), CancellationToken.None);

            result.ShouldBeOfType<DataResponse<PaginatedList<DesignationDTO>>>();

            DataResponse<PaginatedList<DesignationDTO>> response = (DataResponse<PaginatedList<DesignationDTO>>)result;

            response.StatusCode.ShouldBe(StatusCodes.Accepted);

            response.Success.ShouldBe(true);

            response.Message.ShouldBe(Messages.DataFound);

            response.Data.Items.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetAllDesignationListBySearchParam()
        {
            var result = await handler.Handle(new SearchDesignations { Search = "Software" }, CancellationToken.None);

            result.ShouldBeOfType<DataResponse<PaginatedList<DesignationDTO>>>();

            DataResponse<PaginatedList<DesignationDTO>> response = (DataResponse<PaginatedList<DesignationDTO>>)result;

            response.StatusCode.ShouldBe(StatusCodes.Accepted);

            response.Success.ShouldBe(true);

            response.Message.ShouldBe(Messages.DataFound);

            response.Data.Items.Count.ShouldBe(1);
        }

        [Fact]
        public async Task GetAllDesignationListBySearchParamAndPagination()
        {
            var result = await handler.Handle(new SearchDesignations { Search = "Software", Page_No=1, Page_Size=10 }, CancellationToken.None);

            result.ShouldBeOfType<DataResponse<PaginatedList<DesignationDTO>>>();

            DataResponse<PaginatedList<DesignationDTO>> response = (DataResponse<PaginatedList<DesignationDTO>>)result;

            response.StatusCode.ShouldBe(StatusCodes.Accepted);

            response.Success.ShouldBe(true);

            response.Message.ShouldBe(Messages.DataFound);

            response.Data.Items.Count.ShouldBe(1);
        }

        [Fact]
        public async Task GetAllDesignationListBySearchParamNoRecordFound()
        {
            var result = await handler.Handle(new SearchDesignations { Search = "Sales Head" }, CancellationToken.None);

            result.ShouldBeOfType<ErrorResponse>();

            ErrorResponse response = (ErrorResponse)result;

            response.StatusCode.ShouldBe(StatusCodes.BadRequest);

            response.Success.ShouldBe(false);

            response.Errors.Count.ShouldBeGreaterThan(0);
        }

        //[Fact]
        //public async Task GetAllDesignationListExceptionThrown()
        //{
        //    var DesignationMockSet = new Mock<DbSet<Designation>>();
        //    mockContext.Setup(r => r.Designation).Returns(DesignationMockSet.Object);

        //    var result = await handler.Handle(new SearchDesignations(), CancellationToken.None);

        //    result.ShouldBeOfType<ErrorResponse>();

        //    ErrorResponse response = (ErrorResponse)result;

        //    response.StatusCode.ShouldBe(StatusCodes.InternalServerError);

        //    response.Success.ShouldBe(false);

        //    response.Errors.Count.ShouldBeGreaterThan(0);
        //}
    }
}
