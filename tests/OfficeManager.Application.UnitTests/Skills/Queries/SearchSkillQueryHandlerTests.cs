using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Feature.Skills.Queries;
using OfficeManager.Application.UnitTests.Mocks;
using OfficeManager.Application.Wrappers.Concrete;

namespace OfficeManager.Application.UnitTests.Skills.Queries
{
    public class SearchSkillQueryHandlerTests : MockSkillContext
    {
        private readonly SearchSkillQueryHandler handler;
        public SearchSkillQueryHandlerTests()
        {
            handler = new SearchSkillQueryHandler(mockContext.Object, mapper);
        }

        [Fact]
        public async Task GetSkillList()
        {
            var result = await handler.Handle(new SearchSkills(), CancellationToken.None);

            result.ShouldBeOfType<DataResponse<PaginatedList<SkillDTO>>>();

            DataResponse<PaginatedList<SkillDTO>> response = (DataResponse<PaginatedList<SkillDTO>>)result;

            response.StatusCode.ShouldBe(StatusCodes.Accepted);

            response.Success.ShouldBe(true);

            response.Message.ShouldBe(Messages.DataFound);

            response.Data.TotalCount.ShouldBe(2);
        }

        [Fact]
        public async Task GetSkillListBySearchParam()
        {
            var result = await handler.Handle(new SearchSkills { Search = "Project Management" }, CancellationToken.None);

            result.ShouldBeOfType<DataResponse<PaginatedList<SkillDTO>>>();

            DataResponse<PaginatedList<SkillDTO>> response = (DataResponse<PaginatedList<SkillDTO>>)result;

            response.StatusCode.ShouldBe(StatusCodes.Accepted);

            response.Success.ShouldBe(true);

            response.Message.ShouldBe(Messages.DataFound);

            response.Data.TotalCount.ShouldBe(1);
        }

        [Fact]
        public async Task GetSkillListBySearchParamNoRecordFound()
        {
            var result = await handler.Handle(new SearchSkills { Search = "Test" }, CancellationToken.None);

            result.ShouldBeOfType<DataResponse<PaginatedList<SkillDTO>>>();

            DataResponse<PaginatedList<SkillDTO>> response = (DataResponse<PaginatedList<SkillDTO>>)result;

            response.StatusCode.ShouldBe(StatusCodes.Accepted);

            response.Success.ShouldBe(true);

            response.Message.ShouldBe(Messages.NoDataFound);

            response.Data.TotalCount.ShouldBe(0);
        }

        [Fact]
        public async Task GetSkillListExceptionThrown()
        {
            var SkillMockSet = new Mock<DbSet<Skill>>();
            mockContext.Setup(r => r.Skill).Returns(SkillMockSet.Object);

            var result = await handler.Handle(new SearchSkills(), CancellationToken.None);

            result.ShouldBeOfType<ErrorResponse>();

            ErrorResponse response = (ErrorResponse)result;

            response.StatusCode.ShouldBe(StatusCodes.InternalServerError);

            response.Success.ShouldBe(false);

            response.Errors.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task GetSkillList_CheckPaging()
        {
            mockContext.Setup(r => r.Skill).Returns(GetTenSkills().AsQueryable().BuildMockDbSet().Object);

            var result = await handler.Handle(new SearchSkills { Page_No = 1, Page_Size = 5 }, CancellationToken.None);

            result.StatusCode.ShouldBe(StatusCodes.Accepted);

            result.ShouldBeOfType<DataResponse<PaginatedList<SkillDTO>>>();

            DataResponse<PaginatedList<SkillDTO>> response = (DataResponse<PaginatedList<SkillDTO>>)result;

            response.Success.ShouldBe(true);

            response.Message.ShouldBe(Messages.DataFound);

            response.Data.TotalCount.ShouldBe(10);

            response.Data.HasNextPage.ShouldBe(true);

            response.Data.TotalPages.ShouldBe(2);
        }

        [Fact]
        public async Task GetSkillList_NextPage_NoRecordFound()
        {
            var result = await handler.Handle(new SearchSkills { Page_No = 2 }, CancellationToken.None);

            result.ShouldBeOfType<ErrorResponse>();

            result.ShouldBeOfType<ErrorResponse>();

            ErrorResponse response = (ErrorResponse)result;

            response.Success.ShouldBe(false);

            response.Errors.Count.ShouldBe(2);
        }
    }
}
