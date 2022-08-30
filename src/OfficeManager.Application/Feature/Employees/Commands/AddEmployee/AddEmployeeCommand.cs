using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Employees.Commands.AddEmployee
{
    public record AddEmployeeCommand : IRequest<Response<object>>
    {
        public int employeeId { get; init; }
        public int employeeNo { get; init; }
        public string employeeName { get; init; } = string.Empty;
        public string email { get; init; } = string.Empty;
        public int roleId { get; init; }
        public int departmentId { get; init; }
        public int designationId { get; init; }
        public DateTime dateOfBirth { get; init; }
        public DateTime dateOfJoining { get; init; }
        public List<EmployeeSkill> skills { get; init; } = new List<EmployeeSkill>();
    }

    public class AddEmployeeCommandHandler : IRequestHandler<AddEmployeeCommand, Response<object>>
    {
        private readonly IApplicationDbContext context;

        public AddEmployeeCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Response<object>> Handle(AddEmployeeCommand request,CancellationToken cancellationToken)
        {
            Response<object> response = new Response<object>();

            context.BeginTransaction();

            Employee employee = new Employee
            {
                EmployeeNo = request.employeeNo,
                EmployeeName = request.employeeName,
                Email = request.email,
                DateOfBirth = request.dateOfBirth,
                DateOfJoining = request.dateOfJoining,
                DepartmentId = request.departmentId,
                DesignationId = request.designationId
            };

            context.Employees.Add(employee);
            await context.SaveChangesAsync(cancellationToken);

            UserMaster user = new UserMaster
            {
                EmployeeID = employee.Id,
                Email = request.email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Atharva@123")
            };

            context.Users.Add(user);

            foreach (EmployeeSkill skill in request.skills)
            {
                var existingSkill = context.EmployeeSkills.FirstOrDefault(empSk => empSk.skillId == skill.skillId && empSk.EmployeeId == request.employeeId);
                if (existingSkill == null)
                {
                    skill.EmployeeId = employee.Id;
                    context.EmployeeSkills.Add(skill);
                }
                else
                {
                    existingSkill.skillId = skill.skillId;
                    existingSkill.levelId = skill.levelId;
                    existingSkill.rateId = skill.rateId;
                }
            }
            await context.SaveChangesAsync(cancellationToken);

            UserRoleMapping userRole = new UserRoleMapping
            {
                UserId = user.Id,
                RoleId = request.roleId
            };

            context.UserRoleMapping.Add(userRole);
            await context.SaveChangesAsync(cancellationToken);

            context.CommitTransaction();

            response.Message = "Employee added successfully";
            response.StatusCode = "200";
            response.Data = string.Empty;

            return response;
        }
    }
}
