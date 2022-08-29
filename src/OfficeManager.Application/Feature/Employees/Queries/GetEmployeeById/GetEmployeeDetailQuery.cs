using MediatR;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Employees.Queries.GetEmployeeById
{
    public record GetEmployeeDetailQuery(int employeeId) : IRequest<Response<EmployeeDetailDTO>>;

    public class GetEmployeeDetailQueryHandler : IRequestHandler<GetEmployeeDetailQuery, Response<EmployeeDetailDTO>>
    {
        private readonly IApplicationDbContext _context;
        public GetEmployeeDetailQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<EmployeeDetailDTO>> Handle(GetEmployeeDetailQuery request,CancellationToken cancellationToken)
        {
            Response<EmployeeDetailDTO> response = new Response<EmployeeDetailDTO>();
            response._Data = new EmployeeDetailDTO();

            var employeeDetail = _context.Employees.Where(emp => emp.Id == request.employeeId)
                .Select(emp => new EmployeeDetailDTO
                {
                    EmployeeId = emp.Id,
                    Email = emp.Email,
                    EmployeeName = emp.EmployeeName,
                    EmployeeNo = emp.EmployeeNo,
                    DepartmentId = emp.DepartmentId,
                    DesignationId = emp.DesignationId,
                    DateOfBirth = emp.DateOfBirth.Value,
                    DateOfJoining = emp.DateOfJoining,
                    skills = new List<Domain.Entities.EmployeeSkill>(),
                    UserId = _context.Users.FirstOrDefault(u => u.EmployeeID == emp.Id).Id
                }).FirstOrDefault();
            if (employeeDetail != null)
            {
                employeeDetail.RoleId = _context.UserRoleMapping.FirstOrDefault(ur => ur.UserId == employeeDetail.UserId).RoleId;
                response._Data = employeeDetail;
                response._IsSuccess = true;
                response._StatusCode = "200";
                var skills = _context.EmployeeSkills.Where(sk => sk.EmployeeId == employeeDetail.EmployeeId && sk.IsActive == true)
                    .Select(empSkill => new EmployeeSkill
                    {
                        EmployeeId = empSkill.EmployeeId,
                        levelId = empSkill.levelId,
                        skillId = empSkill.skillId,
                        rateId = empSkill.rateId
                    }).ToList();
                if (skills.Any())
                    response._Data.skills = skills;
            }
            else
            {
                response._IsSuccess = false;
                response._StatusCode = "404";
            }

            return response;
        }
    }
}
