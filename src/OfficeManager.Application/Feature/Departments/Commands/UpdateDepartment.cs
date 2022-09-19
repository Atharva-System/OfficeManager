using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.Employees.Commands
{
    public record UpdateDepartment : IRequest<Response<object>>
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; } = string.Empty;
        public bool isActive { get; set; } = false;
    }

    public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartment, Response<object>>
    {
        private readonly IApplicationDbContext Context;

        public UpdateDepartmentCommandHandler(IApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<Response<object>> Handle(UpdateDepartment request, CancellationToken cancellationToken)
        {
            Response<object> response = new Response<object>();

            Context.BeginTransaction();

            Department department = Context.Department.FirstOrDefault(x => x.Id == request.id);
            if (department != null)
            {
                if (Context.Department.Any(x => x.Id != request.id && x.Name == request.name))
                {
                    response.Message = Messages.DepartmentNameExists;
                    response.StatusCode = StausCodes.BadRequest;
                    response.Data = string.Empty;
                    return response;
                }
                department.Name = request.name;
                department.Description = request.description;
                department.IsActive = request.isActive;

                Context.Department.Update(department);
                await Context.SaveChangesAsync(cancellationToken);
                Context.CommitTransaction();
                response.Message = Messages.UpdatedSuccessfully;
                response.StatusCode = StausCodes.Accepted;
                response.Data = string.Empty;
            }
            else
            {
                response.Message = Messages.NotFound;
                response.StatusCode = StausCodes.NotFound;
                response.Data = string.Empty;
            }
            return response;
        }
    }
}