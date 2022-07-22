using MediatR;
using OfficeManager.Application.Common.Exceptions;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Common.Security;

namespace OfficeManager.Application.UserRoles.Command.UpdateUserRoles
{

    [Authorize(Roles = "ADMINISTRATION")]
    public record UpdateUserRoleCommand : IRequest<Result>
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
    }
    public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public UpdateUserRoleCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            var userRole = _context.UserRole.FirstOrDefault(r => r.Id == request.Id);
            if(userRole == null)
            {
                throw new NotFoundException();
            }
            userRole.Title = request.Title;
            userRole.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success("User role updated successfully.");
        }
    }
}
