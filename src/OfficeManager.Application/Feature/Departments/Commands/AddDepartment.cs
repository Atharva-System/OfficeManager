using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.Employees.Commands
{
    public record AddDepartment : IRequest<Response<object>>
    {
        public string name { get; set; }
        public string description { get; set; } = string.Empty;
        public bool isActive { get; set; } = false;
    }

    public class AddDepartmentCommandHandler : IRequestHandler<AddDepartment, Response<object>>
    {
        private readonly IApplicationDbContext Context;

        public AddDepartmentCommandHandler(IApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<Response<object>> Handle(AddDepartment request, CancellationToken cancellationToken)
        {
            Response<object> response = new Response<object>();

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

            response.Message = Messages.AddedSuccesfully;
            response.StatusCode = StausCodes.Accepted;
            response.Data = department;
            return response;
        }
    }
}