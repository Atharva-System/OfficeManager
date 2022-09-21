using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Feature.Employees.Commands;
using OfficeManager.Application.UnitTests.Mocks;
using OfficeManager.Application.Wrappers.Concrete;

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

            result.ShouldBeOfType<SuccessResponse>();

            SuccessResponse response = (SuccessResponse)result;

            response.Message.ShouldBe(Messages.UpdatedSuccessfully);

            response.StatusCode.ShouldBe(StatusCodes.Accepted);
        }
    }
}