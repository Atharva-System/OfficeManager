using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.Employees.Commands
{
    public record UpdateDepartment : IRequest<IResponse>
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; } = string.Empty;
        public bool isActive { get; set; } = false;
    }

    public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartment, IResponse>
    {
        private readonly IApplicationDbContext Context;

        public UpdateDepartmentCommandHandler(IApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<IResponse> Handle(UpdateDepartment request, CancellationToken cancellationToken)
        {
            Context.BeginTransaction();

            Department department = Context.Department.FirstOrDefault(x => x.Id == request.id);
            if (department != null)
            {
                if (Context.Department.Any(x => x.Id != request.id && x.Name == request.name))
                {
                    return new ErrorResponse(StatusCodes.BadRequest,Messages.DepartmentNameExists);
                }
                department.Name = request.name;
                department.Description = request.description;
                department.IsActive = request.isActive;

                Context.Department.Update(department);
                await Context.SaveChangesAsync(cancellationToken);
                Context.CommitTransaction();
                return new SuccessResponse(StatusCodes.Accepted,Messages.UpdatedSuccessfully);
            }
            return new ErrorResponse(StatusCodes.NotFound,Messages.NoDataFound);
        }
    }
}