using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Skills.Commands.CreateSkill
{
    public record CreateSkillCommand : IRequest<Response<object>>
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
    }
    public class CreateSkillCommandHandler : IRequestHandler<CreateSkillCommand,Response<object>>
    {
        private readonly IApplicationDbContext context;
        public CreateSkillCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Response<object>> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
        {
            Skill skill = new Skill();
            skill.Name = request.Name;
            skill.Description = request.Description;
            skill.IsActive = true;

            await context.Skill.AddAsync(skill);
            await context.SaveChangesAsync(cancellationToken);
            context.CommitTransaction();

            Response<object> response = new Response<object>();
            response.Data = skill;
            response.IsSuccess = true;
            response.Message = "Skill is added";
            response.StatusCode = "200";

            return response;
        }
    }
}
