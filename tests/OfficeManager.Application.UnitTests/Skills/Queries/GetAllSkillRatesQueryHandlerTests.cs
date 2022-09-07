using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Feature.Skills.Queries;
using OfficeManager.Application.UnitTests.Mocks;

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

            result.ShouldBeOfType<Response<List<SkillRate>>>();

            result.StatusCode.ShouldBe(StausCodes.Accepted);

            result.IsSuccess.ShouldBe(true);

            result.Message.ShouldBe(Messages.DataFound);

            result.Data.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetAllSkillRateListExceptionThrown()
        {
            var SkillRateMockSet = new Mock<DbSet<SkillRate>>();
            mockContext.Setup(r => r.SkillRate).Returns(SkillRateMockSet.Object);

            var result = await handler.Handle(new GetAllSkillRates(), CancellationToken.None);

            result.ShouldBeOfType<Response<List<SkillRate>>>();

            result.StatusCode.ShouldBe(StausCodes.InternalServerError);

            result.IsSuccess.ShouldBe(false);

            result.Message.ShouldBe(Messages.IssueWithData);

            result.Errors.Count.ShouldBeGreaterThan(0);

            result.Data.Count.ShouldBe(0);
        }
    }
    
}
