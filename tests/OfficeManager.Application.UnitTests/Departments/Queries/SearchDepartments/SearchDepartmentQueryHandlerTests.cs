using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.UnitTests.Mocks;
using OfficeManager.Application.Feature.Departments.Queries;

namespace OfficeManager.Application.UnitTests.Departments.Queries.SearchDepartments
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
            var result = await handler.Handle(new Feature.Departments.Queries.SearchDepartments(string.Empty), CancellationToken.None);

            result.ShouldBeOfType<Response<List<DepartmentDTO>>>();

            result.StatusCode.ShouldBe("200");

            result.IsSuccess.ShouldBe(true);

            result.Message.ShouldBe("Data found!");

            result.Data.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetAllDepartmentListBySearchParam()
        {
            var result = await handler.Handle(new Feature.Departments.Queries.SearchDepartments("Anal"), CancellationToken.None);

            result.ShouldBeOfType<Response<List<DepartmentDTO>>>();

            result.StatusCode.ShouldBe("200");

            result.IsSuccess.ShouldBe(true);

            result.Message.ShouldBe("Data found!");

            result.Data.Count.ShouldBe(1);
        }

        [Fact]
        public async Task GetAllDepartmentListBySearchParamNoRecordFound()
        {
            var result = await handler.Handle(new Feature.Departments.Queries.SearchDepartments("HR"), CancellationToken.None);

            result.ShouldBeOfType<Response<List<DepartmentDTO>>>();

            result.StatusCode.ShouldBe("200");

            result.IsSuccess.ShouldBe(true);

            result.Message.ShouldBe("No Data found!");

            result.Data.Count.ShouldBe(0);
        }

        [Fact]
        public async Task GetAllDepartmentListExceptionThrown()
        {
            var DepartmentMockSet = new Mock<DbSet<Department>>();
            mockContext.Setup(r => r.Department).Returns(DepartmentMockSet.Object);

            var result = await handler.Handle(new Feature.Departments.Queries.SearchDepartments(string.Empty), CancellationToken.None);

            result.ShouldBeOfType<Response<List<DepartmentDTO>>>();

            result.StatusCode.ShouldBe("500");

            result.IsSuccess.ShouldBe(false);

            result.Message.ShouldBe("There is some issue with the data!");

            result.Errors.Count.ShouldBeGreaterThan(0);
        }
    }
}
