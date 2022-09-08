using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Feature.Designations.Queries;
using OfficeManager.Application.UnitTests.Mocks;

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

            result.ShouldBeOfType<Response<List<DesignationDTO>>>();

            result.StatusCode.ShouldBe(StausCodes.Accepted);

            result.IsSuccess.ShouldBe(true);

            result.Message.ShouldBe(Messages.DataFound);

            result.Data.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetAllDesignationListExceptionThrown()
        {
            var DesignationMockSet = new Mock<DbSet<Designation>>();
            mockContext.Setup(r => r.Designation).Returns(DesignationMockSet.Object);

            var result = await handler.Handle(new GetAllDesignations(), CancellationToken.None);

            result.ShouldBeOfType<Response<List<DesignationDTO>>>();

            result.StatusCode.ShouldBe(StausCodes.InternalServerError);

            result.IsSuccess.ShouldBe(false);

            result.Message.ShouldBe(Messages.IssueWithData);

            result.Errors.Count.ShouldBeGreaterThan(0);

            result.Data.ShouldBeNull();
        }
    }
}
