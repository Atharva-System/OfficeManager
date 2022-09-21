using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.Employees.Commands
{
    public record DeleteDepartment(int id) : IRequest<IResponse> { }

    public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartment, IResponse>
    {
        private readonly IApplicationDbContext Context;

        public DeleteDepartmentCommandHandler(IApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<IResponse> Handle(DeleteDepartment request, CancellationToken cancellationToken)
        {
            Context.BeginTransaction();

            Department department = Context.Department.FirstOrDefault(x => x.Id == request.id);
            if (department != null)
            {
                Context.Department.Remove(department);
                await Context.SaveChangesAsync(cancellationToken);
                Context.CommitTransaction();
                return new SuccessResponse(StatusCodes.Accepted, Messages.DeletedSuccessfully);
            }
            return new ErrorResponse(StatusCodes.NotFound, Messages.NoDataFound);
        }
    }
}