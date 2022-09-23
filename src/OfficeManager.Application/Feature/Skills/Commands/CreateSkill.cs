using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.Skills.Commands
{
    public record CreateSkill : IRequest<IResponse>
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
    }
    public class CreateSkillCommandHandler : IRequestHandler<CreateSkill, IResponse>
    {
        private readonly IApplicationDbContext Context;
        public CreateSkillCommandHandler(IApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<IResponse> Handle(CreateSkill request, CancellationToken cancellationToken)
        {
            Context.BeginTransaction();
            Skill skill = new Skill();
            skill.Name = request.Name;
            skill.Description = request.Description;
            skill.IsActive = true;

            await Context.Skill.AddAsync(skill);
            await Context.SaveChangesAsync(cancellationToken);
            Context.CommitTransaction();

            return new SuccessResponse(StatusCodes.Accepted, Messages.AddedSuccesfully);
        }
    }
}
