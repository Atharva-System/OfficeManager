using OfficeManager.Application.Feature.ApplicationUsers.Commands;
using OfficeManager.Application.UnitTests.Mocks;

namespace OfficeManager.Application.UnitTests.ApplicationUsers.Commands
{
    public class RegisterUserCommandHandlerTests : MockApplicationUserContext
    {
        private readonly RegisterUserCommandHandler handler;
        public RegisterUserCommandHandlerTests()
        {
            handler = new RegisterUserCommandHandler(mockContext.Object);
        }
    }
}
