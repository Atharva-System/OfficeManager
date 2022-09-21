using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Feature.Skills.Commands;
using OfficeManager.Application.UnitTests.Mocks;
using OfficeManager.Application.Wrappers.Concrete;

namespace OfficeManager.Application.UnitTests.Skills.Commands
{
    public class CreateSkillCommandHandlerTests : MockSkillContext
    {
        private readonly CreateSkillCommandHandler handler;
        public CreateSkillCommandHandlerTests()
        {
            handler = new CreateSkillCommandHandler(mockContext.Object);
        }

        [Fact]
        public async Task When_ValidSkill_Added()
        {
            var result = await handler.Handle(new CreateSkill { Name = "Test Skill" }, CancellationToken.None);

            result.ShouldBeOfType<SuccessResponse>();

            SuccessResponse response = (SuccessResponse)result;

            response.Message.ShouldBe(Messages.AddedSuccesfully);

            response.StatusCode.ShouldBe(StatusCodes.Accepted);

            var skills = mockContext.Object.Skill;

            skills.Count().ShouldBe(3);
        }
    }
}
