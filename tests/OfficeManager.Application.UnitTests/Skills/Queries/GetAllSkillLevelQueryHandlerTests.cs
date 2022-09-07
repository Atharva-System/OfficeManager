using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Feature.Skills.Queries;
using OfficeManager.Application.UnitTests.Mocks;

namespace OfficeManager.Application.UnitTests.Skills.Queries
{
    public class GetAllSkillLevelQueryHandlerTests : MockSkillContext
    {
        private readonly GetAllSkillLevelQueryHandler handler;
        public GetAllSkillLevelQueryHandlerTests()
        {
            handler = new GetAllSkillLevelQueryHandler(mockContext.Object);
        }

        [Fact]
        public async Task GetAllSkillLevelList()
        {
            var result = await handler.Handle(new GetAllSkillLevels(), CancellationToken.None);

            result.ShouldBeOfType<Response<List<SkillLevel>>>();

            result.StatusCode.ShouldBe(StausCodes.Accepted);

            result.IsSuccess.ShouldBe(true);

            result.Message.ShouldBe(Messages.DataFound);

            result.Data.Count.ShouldBe(3);
        }

        [Fact]
        public async Task GetAllSkillLevelListExceptionThrown()
        {
            var SkillLevelMockSet = new Mock<DbSet<SkillLevel>>();
            mockContext.Setup(r => r.SkillLevel).Returns(SkillLevelMockSet.Object);

            var result = await handler.Handle(new GetAllSkillLevels(), CancellationToken.None);

            result.ShouldBeOfType<Response<List<SkillLevel>>>();

            result.StatusCode.ShouldBe(StausCodes.InternalServerError);

            result.IsSuccess.ShouldBe(false);

            result.Message.ShouldBe(Messages.IssueWithData);

            result.Errors.Count.ShouldBeGreaterThan(0);

            result.Data.Count.ShouldBe(0);
        }
    }
}
