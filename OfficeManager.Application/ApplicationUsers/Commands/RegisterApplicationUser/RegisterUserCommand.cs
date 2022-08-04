using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.ApplicationUsers.Commands.RegisterApplicationUser
{
    public record RegisterUserCommand : IRequest<Result>
    {
        public int EmployeeNo { get; set; }
        public int DesignationId { get; set; }
        public int DepartmentId { get; set; }
        public string Email { get; set; }
        public string EmployeeName { get; set; }
        public int roleId { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime DOJ { get; set; }
        public string Password { get; set; }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public RegisterUserCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _context.BeginTransaction();
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
                _context.Employees.Add(employee);

                await _context.SaveChangesAsync(cancellationToken);

                UserMaster user = new UserMaster
                {
                    EmployeeID = employee.Id,
                    Email = request.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
                };
                _context.Users.Add(user);

                await _context.SaveChangesAsync(cancellationToken);

                UserRoleMapping userRole = new UserRoleMapping()
                {
                    UserId = user.Id,
                    RoleId = request.roleId
                };
                _context.UserRoleMapping.Add(userRole);
                await _context.SaveChangesAsync(cancellationToken);

                _context.CommitTransaction();
                return Result.Success($"{employee.EmployeeName} is registered successfully.");
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
