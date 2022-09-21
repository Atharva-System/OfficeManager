using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.Employees.Commands
{
    public record AddDepartment : IRequest<IResponse>
    {
        public string name { get; set; }
        public string description { get; set; } = string.Empty;
        public bool isActive { get; set; } = false;
    }

    public class AddDepartmentCommandHandler : IRequestHandler<AddDepartment, IResponse>
    {
        private readonly IApplicationDbContext Context;

        public AddDepartmentCommandHandler(IApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<IResponse> Handle(AddDepartment request, CancellationToken cancellationToken)
        {
            Context.BeginTransaction();

            Department department = new Department
            {
                Name = request.name,
                Description = request.description,
                IsActive = request.isActive,
            };

            Context.Department.Add(department);
            await Context.SaveChangesAsync(cancellationToken);

            Context.CommitTransaction();

            return new SuccessResponse(StatusCodes.Accepted,Messages.AddedSuccesfully);
        }
    }
}