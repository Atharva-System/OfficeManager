using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Feature.Departments.Queries;
using OfficeManager.Application.UnitTests.Mocks;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;

namespace OfficeManager.Application.UnitTests.Departments.Queries
{
    public class GetAllDepertmentsHandlerTest : MockDepartmentContext
    {
        private readonly GetAllDepartmentsHandler handler;
        public GetAllDepertmentsHandlerTest()
        {
            handler = new GetAllDepartmentsHandler(mockContext.Object, mapper);
        }

        [Fact]
        public async Task GetAllDepartmentList()
        {
            var result = await handler.Handle(new GetAllDepartments(), CancellationToken.None);

            result.ShouldBeOfType<IResponse>();

            var response = (DataResponse<List<DepartmentDTO>>)result;

            response.StatusCode.ShouldBe(StatusCodes.Accepted);

            response.Success.ShouldBe(true);

            response.Data.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetAllDepartmentListExceptionThrown()
        {
            var DepartmentMockSet = new Mock<DbSet<Department>>();
            mockContext.Setup(r => r.Department).Returns(DepartmentMockSet.Object);

            var result = await handler.Handle(new GetAllDepartments(), CancellationToken.None);

            result.ShouldBeOfType<IResponse>();

            var response = (ErrorResponse)result;

            response.StatusCode.ShouldBe(StatusCodes.InternalServerError);

            response.Success.ShouldBe(false);

            response.Errors.Count.ShouldBeGreaterThan(0);
        }
    }
}
