using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Feature.Skills.Queries;
using OfficeManager.Application.UnitTests.Mocks;
using OfficeManager.Application.Wrappers.Concrete;

namespace OfficeManager.Application.UnitTests.Skills.Queries
{
    public class GetAllSkillRatesQueryHandlerTests : MockSkillContext
    {
        private readonly GetAllSkillRatesQueryHandler handler;
        public GetAllSkillRatesQueryHandlerTests()
        {
            handler = new GetAllSkillRatesQueryHandler(mockContext.Object);
        }

        [Fact]
        public async Task GetAllSkillRateList()
        {
            var result = await handler.Handle(new GetAllSkillRates(), CancellationToken.None);

            result.ShouldBeOfType<DataResponse<List<SkillRate>>>();

            DataResponse<List<SkillRate>> response = (DataResponse<List<SkillRate>>)result;

            response.StatusCode.ShouldBe(StatusCodes.Accepted);

            response.Success.ShouldBe(true);

            response.Message.ShouldBe(Messages.DataFound);

            response.Data.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetAllSkillRateListExceptionThrown()
        {
            var SkillRateMockSet = new Mock<DbSet<SkillRate>>();
            mockContext.Setup(r => r.SkillRate).Returns(SkillRateMockSet.Object);

            var result = await handler.Handle(new GetAllSkillRates(), CancellationToken.None);

            result.ShouldBeOfType<ErrorResponse>();

            ErrorResponse response = (ErrorResponse)result;

            response.StatusCode.ShouldBe(StatusCodes.InternalServerError);

            response.Success.ShouldBe(false);

            response.Errors.Count.ShouldBeGreaterThan(0);
        }
    }
    
}
