using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Feature.Employees.Commands;
using OfficeManager.Application.UnitTests.Mocks;

namespace OfficeManager.Application.UnitTests.Skills.Commands
{
    public class AddDepartmentCommandHandlerTests : MockDepartmentContext
    {
        private readonly AddDepartmentCommandHandler handler;

        public AddDepartmentCommandHandlerTests()
        {
            handler = new AddDepartmentCommandHandler(mockContext.Object);
        }

        [Fact]
        public async Task When_ValidDepartment_Added()
        {
            var result = await handler.Handle(new AddDepartment { name = "Admin", description = "Admin", isActive = true }, CancellationToken.None);

            result.ShouldBeOfType<Response<object>>();

            result.Message.ShouldBe(Messages.AddedSuccesfully);

            result.StatusCode.ShouldBe(StausCodes.Accepted);
        }
    }
}