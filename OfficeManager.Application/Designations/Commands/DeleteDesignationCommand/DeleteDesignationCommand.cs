using MediatR;
using OfficeManager.Application.Common.Exceptions;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Designations.Commands.DeleteDesignationCommand
{
    public record DeleteDesignationCommand(Guid id) : IRequest<Result>;
    public class DeleteDesignationCommandHandler : IRequestHandler<DeleteDesignationCommand,Result>
    {
        private readonly IApplicationDbContext _context;
        public DeleteDesignationCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteDesignationCommand request, CancellationToken cancellationToken)
        {
            DesignationMaster designation = _context.DesignationMasters.FirstOrDefault(d => d.Id == request.id);

            if (designation == null)
                throw new NotFoundException();

            _context.DesignationMasters.Remove(designation);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success($"{designation.Name} designation deleted successfully");
        }
    }
}
