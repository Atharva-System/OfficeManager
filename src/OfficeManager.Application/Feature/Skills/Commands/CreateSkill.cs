using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.Skills.Commands
{
    public record CreateSkill : IRequest<Response<object>>
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
    }
    public class CreateSkillCommandHandler : IRequestHandler<CreateSkill, Response<object>>
    {
        private readonly IApplicationDbContext Context;
        public CreateSkillCommandHandler(IApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<Response<object>> Handle(CreateSkill request, CancellationToken cancellationToken)
        {
            Context.BeginTransaction();
            Skill skill = new Skill();
            skill.Name = request.Name;
            skill.Description = request.Description;
            skill.IsActive = true;

            await Context.Skill.AddAsync(skill);
            await Context.SaveChangesAsync(cancellationToken);
            Context.CommitTransaction();

            Response<object> response = new Response<object>();
            response.Data = skill;
            response.IsSuccess = true;
            response.Message = Messages.AddedSuccesfully;
            response.StatusCode = StausCodes.Accepted;

            return response;
        }
    }
}
