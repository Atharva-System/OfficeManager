using OfficeManager.Application.Feature.ApplicationUsers.Commands;
using OfficeManager.Application.UnitTests.Mocks;

namespace OfficeManager.Application.UnitTests.ApplicationUsers.Commands
{
    public class ForgotPasswordCommandHandlerTests : MockApplicationUserContext
    {
        private readonly ForgotPasswordCommandHandler handler;
        public ForgotPasswordCommandHandlerTests()
        {
            handler = new ForgotPasswordCommandHandler(mockContext.Object);
        }

        [Fact]
        public async Task ForgotPassword_Request_Valid()
        {
            var result = await handler.Handle(new ForgotPassword { Email = "test@test.com"}, CancellationToken.None);

            result.ShouldBeOfType<bool>();

            result.ShouldBe(true);
        }
    }
}
