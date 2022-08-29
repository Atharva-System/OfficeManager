using Microsoft.EntityFrameworkCore;
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
            handler = new SearchDesignationsQueryHandler(_mockContext.Object, _mapper);
        }

        [Fact]
        public async Task GetAllDesignationList()
        {
            var result = await handler.Handle(new SearchDesignationsQuery(String.Empty), CancellationToken.None);

            result.ShouldBeOfType<Response<List<DesignationDto>>>();

            result._StatusCode.ShouldBe("200");

            result._IsSuccess.ShouldBe(true);

            result._Message.ShouldBe("Data found!");

            result.Data.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetAllDesignationList_By_SearchParam()
        {
            var result = await handler.Handle(new SearchDesignationsQuery("Software"), CancellationToken.None);

            result.ShouldBeOfType<Response<List<DesignationDto>>>();

            result._StatusCode.ShouldBe("200");

            result._IsSuccess.ShouldBe(true);

            result._Message.ShouldBe("Data found!");

            result.Data.Count.ShouldBe(1);
        }

        [Fact]
        public async Task GetAllDesignationListBy_SearchParam_NoRecordFound()
        {
            var result = await handler.Handle(new SearchDesignationsQuery("Sales Head"), CancellationToken.None);

            result.ShouldBeOfType<Response<List<DesignationDto>>>();

            result._StatusCode.ShouldBe("200");

            result._IsSuccess.ShouldBe(true);

            result._Message.ShouldBe("No Data found!");

            result.Data.Count.ShouldBe(0);
        }

        [Fact]
        public async Task GetAllDesignationList_ExceptionThrown()
        {
            var DesignationMockSet = new Mock<DbSet<Designation>>();
            _mockContext.Setup(r => r.Designation).Returns(DesignationMockSet.Object);

            var result = await handler.Handle(new SearchDesignationsQuery(String.Empty), CancellationToken.None);

            result.ShouldBeOfType<Response<List<DesignationDto>>>();

            result._StatusCode.ShouldBe("500");

            result._IsSuccess.ShouldBe(false);

            result._Message.ShouldBe("There is some issue with the data!");

            result._Errors.Count.ShouldBeGreaterThan(0);

            result.Data.ShouldBeNull();
        }
    }
}
