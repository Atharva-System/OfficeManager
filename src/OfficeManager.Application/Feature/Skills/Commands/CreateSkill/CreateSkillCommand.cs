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
        private readonly IApplicationDbContext _context;
        public CreateSkillCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<object>> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
        {
            Skill skill = new Skill();
            skill.Name = request.Name;
            skill.Description = request.Description;
            skill.IsActive = true;

            await _context.Skill.AddAsync(skill);
            await _context.SaveChangesAsync(cancellationToken);
            _context.CommitTransaction();

            Response<object> response = new Response<object>();
            response._Data = skill;
            response._IsSuccess = true;
            response._Message = "Skill is added";
            response._StatusCode = "200";

            return response;
        }
    }
}
