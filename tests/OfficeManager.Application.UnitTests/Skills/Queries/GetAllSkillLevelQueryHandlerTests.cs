using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Feature.Skills.Queries;
using OfficeManager.Application.UnitTests.Mocks;
using OfficeManager.Application.Wrappers.Concrete;

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

            result.ShouldBeOfType<DataResponse<List<SkillLevel>>>();

            DataResponse<List<SkillLevel>> response = (DataResponse<List<SkillLevel>>)result;

            response.StatusCode.ShouldBe(StatusCodes.Accepted);

            response.Success.ShouldBe(true);

            response.Message.ShouldBe(Messages.DataFound);

            response.Data.Count.ShouldBe(3);
        }

        //[Fact]
        //public async Task GetAllSkillLevelListExceptionThrown()
        //{
        //    var SkillLevelMockSet = new Mock<DbSet<SkillLevel>>();
        //    mockContext.Setup(r => r.SkillLevel).Returns(SkillLevelMockSet.Object);

        //    var result = await handler.Handle(new GetAllSkillLevels(), CancellationToken.None);

        //    result.ShouldBeOfType<ErrorResponse>();

        //    ErrorResponse response = (ErrorResponse)result;

        //    response.StatusCode.ShouldBe(StatusCodes.InternalServerError);

        //    response.Success.ShouldBe(false);

        //    response.Errors.Count.ShouldBeGreaterThan(0);
        //}
    }
}
