using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Designations.Commands.CreateDesignationCommand
{
    public record CreateDesignationCommand : IRequest<Result>
    {
        public string Name { get; init; }
    }

    public class CreateDesignationCommandHandler : IRequestHandler<CreateDesignationCommand,Result>
    {
        private readonly IApplicationDbContext _context;
        public CreateDesignationCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(CreateDesignationCommand request,CancellationToken cancellationToken)
        {
            DesignationMaster designation = new DesignationMaster
            {
                Name = request.Name
            };

            _context.DesignationMasters.Add(designation);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success("Designation added successfully");
        }
    }
}
