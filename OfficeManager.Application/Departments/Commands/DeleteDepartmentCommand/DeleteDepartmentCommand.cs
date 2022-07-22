using MediatR;
using OfficeManager.Application.Common.Exceptions;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Common.Security;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Departments.Commands.DeleteDepartmentCommand
{
    [Authorize(Roles = "ADMINISTRATION")]
    public record DeleteDepartmentCommand(Guid id) : IRequest<Result>;

    public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public DeleteDepartmentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            DepartmentMaster department = _context.DepartmentMasters.FirstOrDefault(d => d.Id == request.id);
            if (department == null)
                throw new NotFoundException();
            _context.DepartmentMasters.Remove(department);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success("Department deleted successfully.");
        }
    }
}
