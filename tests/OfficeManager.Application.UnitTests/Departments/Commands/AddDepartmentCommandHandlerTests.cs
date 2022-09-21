using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Feature.Employees.Commands;
using OfficeManager.Application.UnitTests.Mocks;
using OfficeManager.Application.Wrappers.Concrete;

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

            result.ShouldBeOfType<SuccessResponse>();

            SuccessResponse response = (SuccessResponse)result;

            response.Message.ShouldBe(Messages.AddedSuccesfully);

            response.StatusCode.ShouldBe(StatusCodes.Accepted);
        }
    }
}