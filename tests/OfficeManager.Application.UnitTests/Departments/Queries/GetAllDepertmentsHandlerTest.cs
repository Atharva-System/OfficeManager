using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Feature.Departments.Queries;
using OfficeManager.Application.UnitTests.Mocks;

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

            result.ShouldBeOfType<Response<List<DepartmentDTO>>>();

            result.StatusCode.ShouldBe(StausCodes.Accepted);

            result.IsSuccess.ShouldBe(true);

            result.Message.ShouldBe(Messages.DataFound);

            result.Data.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetAllDepartmentListExceptionThrown()
        {
            var DepartmentMockSet = new Mock<DbSet<Department>>();
            mockContext.Setup(r => r.Department).Returns(DepartmentMockSet.Object);

            var result = await handler.Handle(new GetAllDepartments(), CancellationToken.None);

            result.ShouldBeOfType<Response<List<DepartmentDTO>>>();

            result.StatusCode.ShouldBe(StausCodes.InternalServerError);

            result.IsSuccess.ShouldBe(false);

            result.Message.ShouldBe(Messages.IssueWithData);

            result.Errors.Count.ShouldBeGreaterThan(0);
        }
    }
}
