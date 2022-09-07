using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.Employees.Commands
{
    public record UpdateEmployee : IRequest<Response<object>>
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

    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployee, Response<object>>
    {
        private readonly IApplicationDbContext Context;
        public UpdateEmployeeCommandHandler(IApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<Response<object>> Handle(UpdateEmployee request, CancellationToken cancellationToken)
        {
            Response<object> response = new Response<object>();

            Employee employee = Context.Employees.FirstOrDefault(emp => emp.Id == request.employeeId);
            if (employee == null)
            {
                response.Errors.Add(Messages.NoDataFound);
                response.IsSuccess = false;
                response.StatusCode = StausCodes.NotFound;
                return response;
            }
            Context.BeginTransaction();

            employee.EmployeeNo = request.employeeNo;
            employee.EmployeeName = request.employeeName;
            employee.Email = request.email;
            employee.DateOfBirth = request.dateOfBirth;
            employee.DateOfJoining = request.dateOfJoining;
            employee.DepartmentId = request.departmentId;
            employee.DesignationId = request.designationId;

            await Context.SaveChangesAsync(cancellationToken);

            UserRoleMapping userRole = Context.UserRoleMapping.FirstOrDefault(ur => ur.Users.EmployeeID == request.employeeId);
            if (userRole != null)
            {
                userRole.RoleId = request.roleId;
                await Context.SaveChangesAsync(cancellationToken);
            }

            List<EmployeeSkill> skillList = Context.EmployeeSkills.Where(empSk => empSk.EmployeeId == employee.Id).ToList();
            skillList.ForEach(sk =>
            {
                sk.IsActive = false;
            });

            await Context.SaveChangesAsync(cancellationToken);

            foreach (EmployeeSkill skill in request.skills)
            {
                var existingSkill = Context.EmployeeSkills.FirstOrDefault(empSk => empSk.skillId == skill.skillId && empSk.EmployeeId == request.employeeId);
                if (existingSkill == null)
                {
                    skill.EmployeeId = request.employeeId;
                    Context.EmployeeSkills.Add(skill);
                }
                else
                {
                    existingSkill.skillId = skill.skillId;
                    existingSkill.levelId = skill.levelId;
                    existingSkill.rateId = skill.rateId;
                    existingSkill.IsActive = true;
                }
            }


            await Context.SaveChangesAsync(cancellationToken);

            Context.CommitTransaction();
            response.Message = Messages.AddedSuccesfully;
            response.StatusCode = StausCodes.Accepted;
            response.Data = string.Empty;

            return response;
        }
    }
}
