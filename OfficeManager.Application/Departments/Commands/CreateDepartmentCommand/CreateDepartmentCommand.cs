using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Common.Security;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Departments.Commands.CreateDepartmentCommand
{
    [Authorize(Roles = "ADMINISTRATION")]
    public record CreateDepartmentCommand : IRequest<Result>
    {
        public string Name { get; init; }
        public string Description { get; init; }
    }

    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public CreateDepartmentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var department = new DepartmentMaster
                {
                    Name = request.Name,
                    Description = request.Description
                };

                _context.DepartmentMasters.Add(department);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("Department created successfully.");
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
