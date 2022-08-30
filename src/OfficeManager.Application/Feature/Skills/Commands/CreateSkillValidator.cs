using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.Skills.Commands
{
    public class CreateSkillValidator : AbstractValidator<CreateSkill>
    {
        private readonly IApplicationDbContext context;
        public CreateSkillValidator(IApplicationDbContext context)
        {
            this.context = context;
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name is acceeding maximum length of 100 characters.")
                .MustAsync(BeUniqueSkill).WithMessage("Specified skill already exists");
        }

        public async Task<bool> BeUniqueSkill(string skillName, CancellationToken cancellationToken)
        {
            var skill = await context.Skill.FirstOrDefaultAsync(s => s.Name == skillName);
            return skill is null;
        }
    }
}
