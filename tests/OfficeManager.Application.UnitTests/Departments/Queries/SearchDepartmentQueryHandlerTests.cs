using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.UnitTests.Mocks;
using OfficeManager.Application.Feature.Departments.Queries;

namespace OfficeManager.Application.UnitTests.Departments.Queries
{
    public class SearchDepartmentQueryHandlerTests : MockDepartmentContext
    {
        private readonly SearchDepartmentQueryHandler handler;
        public SearchDepartmentQueryHandlerTests()
        {
            handler = new SearchDepartmentQueryHandler(mockContext.Object, mapper);
        }

        [Fact]
        public async Task GetAllDepartmentList()
        {
            var result = await handler.Handle(new SearchDepartments(string.Empty), CancellationToken.None);

            result.ShouldBeOfType<Response<List<DepartmentDTO>>>();

            result.StatusCode.ShouldBe(StausCodes.Accepted);

            result.IsSuccess.ShouldBe(true);

            result.Message.ShouldBe(Messages.DataFound);

            result.Data.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetAllDepartmentListBySearchParam()
        {
            var result = await handler.Handle(new SearchDepartments("Anal"), CancellationToken.None);

            result.ShouldBeOfType<Response<List<DepartmentDTO>>>();

            result.StatusCode.ShouldBe(StausCodes.Accepted);

            result.IsSuccess.ShouldBe(true);

            result.Message.ShouldBe(Messages.DataFound);

            result.Data.Count.ShouldBe(1);
        }

        [Fact]
        public async Task GetAllDepartmentListBySearchParamNoRecordFound()
        {
            var result = await handler.Handle(new SearchDepartments("HR"), CancellationToken.None);

            result.ShouldBeOfType<Response<List<DepartmentDTO>>>();

            result.StatusCode.ShouldBe(StausCodes.Accepted);

            result.IsSuccess.ShouldBe(true);

            result.Message.ShouldBe(Messages.NoDataFound);

            result.Data.Count.ShouldBe(0);
        }

        [Fact]
        public async Task GetAllDepartmentListExceptionThrown()
        {
            var DepartmentMockSet = new Mock<DbSet<Department>>();
            mockContext.Setup(r => r.Department).Returns(DepartmentMockSet.Object);

            var result = await handler.Handle(new SearchDepartments(string.Empty), CancellationToken.None);

            result.ShouldBeOfType<Response<List<DepartmentDTO>>>();

            result.StatusCode.ShouldBe(StausCodes.InternalServerError);

            result.IsSuccess.ShouldBe(false);

            result.Message.ShouldBe(Messages.IssueWithData);

            result.Errors.Count.ShouldBeGreaterThan(0);
        }
    }
}
