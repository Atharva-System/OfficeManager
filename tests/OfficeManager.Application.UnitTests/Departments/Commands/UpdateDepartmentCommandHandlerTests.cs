using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Feature.Employees.Commands;
using OfficeManager.Application.UnitTests.Mocks;

namespace OfficeManager.Application.UnitTests.Skills.Commands
{
    public class UpdateDepartmentCommandHandlerTests : MockDepartmentContext
    {
        private readonly UpdateDepartmentCommandHandler handler;

        public UpdateDepartmentCommandHandlerTests()
        {
            handler = new UpdateDepartmentCommandHandler(mockContext.Object);
        }

        [Fact]
        public async Task When_ValidDepartment_Updated()
        {
            var result = await handler.Handle(new UpdateDepartment { id = 1, name = ".Net", description = "", isActive = false }, CancellationToken.None);

            result.ShouldBeOfType<Response<object>>();

            result.Message.ShouldBe(Messages.UpdatedSuccessfully);

            result.StatusCode.ShouldBe(StausCodes.Accepted);
        }
    }
}