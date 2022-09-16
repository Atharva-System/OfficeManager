using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.UnitTests.Mocks;
using OfficeManager.Application.Feature.Departments.Queries;

namespace OfficeManager.Application.UnitTests.Departments.Queries
{
    public class SearchDepartmentQueryHandlerTests : MockDepartmentContext
    {
        private readonly SearchDepartmentHandler handler;
        public SearchDepartmentQueryHandlerTests()
        {
            handler = new SearchDepartmentHandler(mockContext.Object, mapper);
        }

        [Fact]
        public async Task SearchAllDepartmentList()
        {
            var result = await handler.Handle(new SearchDepartments { }, CancellationToken.None);

            result.ShouldBeOfType<Response<PaginatedList<DepartmentDTO>>>();

            result.StatusCode.ShouldBe(StausCodes.Accepted);

            result.IsSuccess.ShouldBe(true);

            result.Message.ShouldBe(Messages.DataFound);

            result.Data.Items.Count.ShouldBe(2);
        }

        [Fact]
        public async Task SearchAllDepartmentListBySearchParam()
        {
            var result = await handler.Handle(new SearchDepartments { search = "Anal" }, CancellationToken.None);

            result.ShouldBeOfType<Response<PaginatedList<DepartmentDTO>>>();

            result.StatusCode.ShouldBe(StausCodes.Accepted);

            result.IsSuccess.ShouldBe(true);

            result.Message.ShouldBe(Messages.DataFound);

            result.Data.Items.Count.ShouldBe(1);
        }

        [Fact]
        public async Task SearchAllDepartmentListBySearchParamNoRecordFound()
        {
            var result = await handler.Handle(new SearchDepartments { search = "HR" }, CancellationToken.None);

            result.ShouldBeOfType<Response<PaginatedList<DepartmentDTO>>>();

            result.StatusCode.ShouldBe(StausCodes.Accepted);

            result.IsSuccess.ShouldBe(true);

            result.Message.ShouldBe(Messages.NoDataFound);

            result.Data.Items.Count.ShouldBe(0);
        }

        [Fact]
        public async Task SearchAllDepartmentListBySearchParamsAndPagination()
        {
            var result = await handler.Handle(new SearchDepartments { search = "HR", Page_No=1,Page_Size=10 }, CancellationToken.None);

            result.ShouldBeOfType<Response<PaginatedList<DepartmentDTO>>>();

            result.StatusCode.ShouldBe(StausCodes.Accepted);

            result.IsSuccess.ShouldBe(true);

            result.Message.ShouldBe(Messages.NoDataFound);

            result.Data.Items.Count.ShouldBe(0);
        }

        [Fact]
        public async Task SearchAllDepartmentListExceptionThrown()
        {
            var DepartmentMockSet = new Mock<DbSet<Department>>();
            mockContext.Setup(r => r.Department).Returns(DepartmentMockSet.Object);

            var result = await handler.Handle(new SearchDepartments(), CancellationToken.None);

            result.ShouldBeOfType<Response<PaginatedList<DepartmentDTO>>>();

            result.StatusCode.ShouldBe(StausCodes.InternalServerError);

            result.IsSuccess.ShouldBe(false);

            result.Message.ShouldBe(Messages.IssueWithData);

            result.Errors.Count.ShouldBeGreaterThan(0);
        }
    }
}
