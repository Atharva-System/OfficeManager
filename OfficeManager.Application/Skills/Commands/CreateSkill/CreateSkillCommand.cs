using MediatR;
using OfficeManager.Application.Common.Models;

namespace OfficeManager.Application.Skills.Commands.CreateSkill
{
    public record CreateSkillCommand : IRequest<Response<object>>
    {
        public string Name { get; init; }
    }
}
