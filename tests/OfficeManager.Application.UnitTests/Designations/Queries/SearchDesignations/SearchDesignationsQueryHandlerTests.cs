using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Designations.Queries.SearchDesignations;
using OfficeManager.Application.UnitTests.Mocks;

namespace OfficeManager.Application.UnitTests.Designations.Queries.SearchDesignations
{
    public class SearchDesignationsQueryHandlerTests : MockDesignationsContext
    {
        private readonly SearchDesignationsQueryHandler handler;
        public SearchDesignationsQueryHandlerTests()
        {
            handler = new SearchDesignationsQueryHandler(mockContext.Object, mapper);
        }

        [Fact]
        public async Task GetAllDesignationList()
        {
            var result = await handler.Handle(new SearchDesignationsQuery(String.Empty), CancellationToken.None);

            result.ShouldBeOfType<Response<List<DesignationDTO>>>();

            result.StatusCode.ShouldBe("200");

            result.IsSuccess.ShouldBe(true);

            result.Message.ShouldBe("Data found!");

            result.Data.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetAllDesignationListBySearchParam()
        {
            var result = await handler.Handle(new SearchDesignationsQuery("Software"), CancellationToken.None);

            result.ShouldBeOfType<Response<List<DesignationDTO>>>();

            result.StatusCode.ShouldBe("200");

            result.IsSuccess.ShouldBe(true);

            result.Message.ShouldBe("Data found!");

            result.Data.Count.ShouldBe(1);
        }

        [Fact]
        public async Task GetAllDesignationListBySearchParamNoRecordFound()
        {
            var result = await handler.Handle(new SearchDesignationsQuery("Sales Head"), CancellationToken.None);

            result.ShouldBeOfType<Response<List<DesignationDTO>>>();

            result.StatusCode.ShouldBe("200");

            result.IsSuccess.ShouldBe(true);

            result.Message.ShouldBe("No Data found!");

            result.Data.Count.ShouldBe(0);
        }

        [Fact]
        public async Task GetAllDesignationListExceptionThrown()
        {
            var DesignationMockSet = new Mock<DbSet<Designation>>();
            mockContext.Setup(r => r.Designation).Returns(DesignationMockSet.Object);

            var result = await handler.Handle(new SearchDesignationsQuery(String.Empty), CancellationToken.None);

            result.ShouldBeOfType<Response<List<DesignationDTO>>>();

            result.StatusCode.ShouldBe("500");

            result.IsSuccess.ShouldBe(false);

            result.Message.ShouldBe("There is some issue with the data!");

            result.Errors.Count.ShouldBeGreaterThan(0);

            result.Data.ShouldBeNull();
        }
    }
}
