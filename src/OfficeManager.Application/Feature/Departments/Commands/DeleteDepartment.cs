using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.Employees.Commands
{
    public record DeleteDepartment(int id) : IRequest<Response<object>> { }

    public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartment, Response<object>>
    {
        private readonly IApplicationDbContext Context;

        public DeleteDepartmentCommandHandler(IApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<Response<object>> Handle(DeleteDepartment request, CancellationToken cancellationToken)
        {
            Response<object> response = new Response<object>();

            Context.BeginTransaction();

            Department department = Context.Department.FirstOrDefault(x => x.Id == request.id);
            if (department != null)
            {
                Context.Department.Remove(department);
                await Context.SaveChangesAsync(cancellationToken);
                Context.CommitTransaction();
                response.Message = Messages.DeletedSuccessfully;
                response.StatusCode = StausCodes.Accepted;
                response.Data = string.Empty;
            }
            else
            {
                response.Message = Messages.NoDataFound;
                response.StatusCode = StausCodes.NotFound;
                response.Data = string.Empty;
            }
            return response;
        }
    }
}