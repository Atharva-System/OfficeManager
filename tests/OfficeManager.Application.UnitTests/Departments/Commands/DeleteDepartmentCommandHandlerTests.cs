using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Feature.Employees.Commands;
using OfficeManager.Application.UnitTests.Mocks;

namespace OfficeManager.Application.UnitTests.Skills.Commands
{
    public class DeleteDepartmentCommandHandlerTests : MockDepartmentContext
    {
        private readonly DeleteDepartmentCommandHandler handler;
        public DeleteDepartmentCommandHandlerTests()
        {
            handler = new DeleteDepartmentCommandHandler(mockContext.Object);
        }

        [Fact]
        public async Task When_ValidDepartment_Deleteed()
        {
            var result = await handler.Handle(new DeleteDepartment(1), CancellationToken.None);

            result.ShouldBeOfType<Response<object>>();

            result.Message.ShouldBe(Messages.DeletedSuccessfully);

            result.StatusCode.ShouldBe(StausCodes.Accepted);
        }
    }
}
