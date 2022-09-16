using OfficeManager.Application.Feature.ApplicationUsers.Commands;
using OfficeManager.Application.UnitTests.Mocks;

namespace OfficeManager.Application.UnitTests.ApplicationUsers.Commands
{
    public class ForgotPasswordConfirmationCommandHandlerTests : MockApplicationUserContext
    {
        private readonly ForgotPasswordConfirmationCommandHandler handler;
        public ForgotPasswordConfirmationCommandHandlerTests()
        {
            handler = new ForgotPasswordConfirmationCommandHandler(mockContext.Object);
        }

        [Fact]
        public async Task ForgotPasswordConfirm_Request_Valid()
        {
            var result = await handler.Handle(new ForgotPasswordConfirmation { Email = "test@test.com" }, CancellationToken.None);

            result.ShouldBeOfType<bool>();

            result.ShouldBe(true);
        }
    }
}
