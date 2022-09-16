using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.ApplicationUsers.Commands
{
    public record RegisterUser : IRequest<Result>
    {
        public int EmployeeNo { get; set; }
        public int DesignationId { get; set; }
        public int DepartmentId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public int roleId { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime DOJ { get; set; }
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUser, Result>
    {
        private readonly IApplicationDbContext Context;

        public RegisterUserCommandHandler(IApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<Result> Handle(RegisterUser request, CancellationToken cancellationToken)
        {
            try
            {
                Context.BeginTransaction();
                Employee employee = new Employee
                {
                    EmployeeNo = request.EmployeeNo,
                    DesignationId = request.DesignationId,
                    DepartmentId = request.DepartmentId,
                    DateOfJoining = request.DOJ,
                    DateOfBirth = request.DOB,
                    Email = request.Email,
                    EmployeeName = request.EmployeeName
                };
                await Context.Employees.AddAsync(employee);

                await Context.SaveChangesAsync(cancellationToken);

                UserMaster user = new UserMaster
                {
                    EmployeeID = employee.Id,
                    Email = request.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
                };
                await Context.Users.AddAsync(user);

                await Context.SaveChangesAsync(cancellationToken);

                UserRoleMapping userRole = new UserRoleMapping()
                {
                    UserId = user.Id,
                    RoleId = request.roleId
                };
                await Context.UserRoleMapping.AddAsync(userRole);
                await Context.SaveChangesAsync(cancellationToken);

                Context.CommitTransaction();
                return Result.Success(Messages.RegisteredSuccesfully, string.Empty);
            }
            catch (Exception ex)
            {
                List<string> innerExceptions = new List<string>();
                if (ex.InnerException != null)
                {
                    innerExceptions.Add(ex.InnerException.Message);
                }
                return Result.Failure(innerExceptions, ex.Message);
            }
        }
    }
}
