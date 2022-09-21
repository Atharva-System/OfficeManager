using OfficeManager.Application.Feature.Users.Commands;
using OfficeManager.Application.UnitTests.Mocks;

namespace OfficeManager.Application.UnitTests.Users.Commands
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
