using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Employees.Commands.UpdateEmployee
{
    public record UpdateEmployeeCommand : IRequest<Response<object>>
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
        public List<EmployeeSkill> skills { get; init; }
    }

    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand,Response<object>>
    {
        private readonly IApplicationDbContext _context;
        public UpdateEmployeeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<object>> Handle(UpdateEmployeeCommand request,CancellationToken cancellationToken)
        {
            Response<object> response = new Response<object>();
            
            Employee employee = _context.Employees.FirstOrDefault(emp => emp.Id == request.employeeId);
            if(employee == null)
            {
                response._Errors.Add("Data not found.");
                response._IsSuccess = false;
                response._StatusCode = "400";
                return response;
            }
            _context.BeginTransaction();

            employee.EmployeeNo = request.employeeNo;
            employee.EmployeeName = request.employeeName;
            employee.Email = request.email;
            employee.DateOfBirth = request.dateOfBirth;
            employee.DateOfJoining = request.dateOfJoining;
            employee.DepartmentId = request.departmentId;
            employee.DesignationId = request.designationId;

            await _context.SaveChangesAsync(cancellationToken);

            UserRoleMapping userRole = _context.UserRoleMapping.FirstOrDefault(ur => ur.Users.EmployeeID == request.employeeId);
            if(userRole != null)
            {
                userRole.RoleId = request.roleId;
                await _context.SaveChangesAsync(cancellationToken);
            }

            List<EmployeeSkill> skillList = _context.EmployeeSkills.Where(empSk => empSk.EmployeeId == employee.Id).ToList();
            skillList.ForEach(sk =>
            {
                sk.IsActive = false;
            });

            await _context.SaveChangesAsync(cancellationToken);

            foreach (EmployeeSkill skill in request.skills)
            {
                var existingSkill = _context.EmployeeSkills.FirstOrDefault(empSk => empSk.skillId == skill.skillId && empSk.EmployeeId == request.employeeId);
                if(existingSkill == null)
                {
                    skill.EmployeeId = request.employeeId;
                    _context.EmployeeSkills.Add(skill);
                }
                else
                {
                    existingSkill.skillId = skill.skillId;
                    existingSkill.levelId = skill.levelId;
                    existingSkill.rateId = skill.rateId;
                    existingSkill.IsActive = true;
                }
            }


            await _context.SaveChangesAsync(cancellationToken);

            _context.CommitTransaction();
            response._Message = "Employee updated successfully";
            response._StatusCode = "200";
            response._Data = null;

            return response;
        }
    }
}
