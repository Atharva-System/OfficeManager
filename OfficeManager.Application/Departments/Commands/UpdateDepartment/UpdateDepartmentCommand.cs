using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Exceptions;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;

namespace OfficeManager.Application.Departments.Commands.UpdateDepartment
{
    public record UpdateDepartmentCommand : IRequest<Result>
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
    }

    public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand,Result>
    {
        private readonly IApplicationDbContext _context;
        public UpdateDepartmentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var department = await _context.DepartmentMasters.FirstOrDefaultAsync(d => d.Id == request.Id);
                if (department == null)
                {
                    throw new NotFoundException();
                    return Result.Failure(Enumerable.Empty<string>(), "depament not found");
                }
                department.Name = request.Name;
                department.Description = request.Description;
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Department updated successfully!");
            }
            catch (Exception ex)
            {
                List<string> innerExceptions = new List<string>();
                innerExceptions.Add(ex.InnerException.Message);
                return Result.Failure(innerExceptions, ex.Message);
            }
        }
    }
}
