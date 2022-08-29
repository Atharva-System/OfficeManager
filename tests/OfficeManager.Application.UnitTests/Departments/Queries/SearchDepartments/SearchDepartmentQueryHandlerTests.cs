using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Departments.Queries.SearchDepartments;
using OfficeManager.Application.UnitTests.Mocks;

namespace OfficeManager.Application.UnitTests.Departments.Queries.SearchDepartments
{
    public class SearchDepartmentQueryHandlerTests : MockDepartmentContext
    {
        private readonly SearchDepartmentQueryHandler handler;
        public SearchDepartmentQueryHandlerTests()
        {
            handler = new SearchDepartmentQueryHandler(_mockContext.Object, _mapper);
        }

        [Fact]
        public async Task GetAllDepartmentList()
        {
            var result = await handler.Handle(new SearchDepartmentsQuery(String.Empty), CancellationToken.None);

            result.ShouldBeOfType<Response<List<DepartmentDTO>>>();

            result._StatusCode.ShouldBe("200");

            result._IsSuccess.ShouldBe(true);

            result._Message.ShouldBe("Data found!");

            result.Data.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetAllDepartmentList_By_SearchParam()
        {
            var result = await handler.Handle(new SearchDepartmentsQuery("Anal"), CancellationToken.None);

            result.ShouldBeOfType<Response<List<DepartmentDTO>>>();

            result._StatusCode.ShouldBe("200");

            result._IsSuccess.ShouldBe(true);

            result._Message.ShouldBe("Data found!");

            result.Data.Count.ShouldBe(1);
        }

        [Fact]
        public async Task GetAllDepartmentListBy_SearchParam_NoRecordFound()
        {
            var result = await handler.Handle(new SearchDepartmentsQuery("HR"), CancellationToken.None);

            result.ShouldBeOfType<Response<List<DepartmentDTO>>>();

            result._StatusCode.ShouldBe("200");

            result._IsSuccess.ShouldBe(true);

            result._Message.ShouldBe("No Data found!");

            result.Data.Count.ShouldBe(0);
        }

        [Fact]
        public async Task GetAllDepartmentList_ExceptionThrown()
        {
            var DepartmentMockSet = new Mock<DbSet<DepartMent>>();
            _mockContext.Setup(r => r.DepartMent).Returns(DepartmentMockSet.Object);

            var result = await handler.Handle(new SearchDepartmentsQuery(String.Empty), CancellationToken.None);

            result.ShouldBeOfType<Response<List<DepartmentDTO>>>();

            result._StatusCode.ShouldBe("500");

            result._IsSuccess.ShouldBe(false);

            result._Message.ShouldBe("There is some issue with the data!");

            result._Errors.Count.ShouldBeGreaterThan(0);

            result.Data.ShouldBeNull();
        }
    }
}
