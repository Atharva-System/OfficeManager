using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Feature.Designations.Queries;
using OfficeManager.Application.UnitTests.Mocks;
using OfficeManager.Application.Wrappers.Concrete;

namespace OfficeManager.Application.UnitTests.Designations.Queries
{
    public class GetAllDesignationsHandlerTest : MockDesignationsContext
    {
        private readonly GetAllDesignationsHandler handler;
        public GetAllDesignationsHandlerTest()
        {
            handler = new GetAllDesignationsHandler(mockContext.Object, mapper);
        }

        [Fact]
        public async Task GetAllDesignationList()
        {
            var result = await handler.Handle(new GetAllDesignations(), CancellationToken.None);

            result.ShouldBeOfType<DataResponse<List<DesignationDTO>>>();

            result.StatusCode.ShouldBe(StatusCodes.Accepted);

            result.Success.ShouldBe(true);

        }

        [Fact]
        public async Task GetAllDesignationListExceptionThrown()
        {
            var DesignationMockSet = new Mock<DbSet<Designation>>();
            mockContext.Setup(r => r.Designation).Returns(DesignationMockSet.Object);

            var result = await handler.Handle(new GetAllDesignations(), CancellationToken.None);

            result.ShouldBeOfType<ErrorResponse>();

            ErrorResponse response = (ErrorResponse)result;

            response.StatusCode.ShouldBe(StatusCodes.InternalServerError);

            response.Success.ShouldBe(false);

            response.Errors.Count.ShouldBeGreaterThan(0);
        }
    }
}
