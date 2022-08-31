using MediatR;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.Employees.Queries
{
    public record GetEmployeeDetail(int employeeId) : IRequest<Response<EmployeeDetailDTO>>;

    public class GetEmployeeDetailQueryHandler : IRequestHandler<GetEmployeeDetail, Response<EmployeeDetailDTO>>
    {
        private readonly IApplicationDbContext context;
        public GetEmployeeDetailQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Response<EmployeeDetailDTO>> Handle(GetEmployeeDetail request, CancellationToken cancellationToken)
        {
            Response<EmployeeDetailDTO> response = new Response<EmployeeDetailDTO>();
            response.Data = new EmployeeDetailDTO();

            var employeeDetail = context.Employees.Where(emp => emp.Id == request.employeeId)
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
                    skills = new List<EmployeeSkill>(),
                    UserId = context.Users.FirstOrDefault(u => u.EmployeeID == emp.Id).Id
                }).FirstOrDefault();
            if (employeeDetail != null)
            {
                employeeDetail.RoleId = context.UserRoleMapping.FirstOrDefault(ur => ur.UserId == employeeDetail.UserId).RoleId;
                response.Data = employeeDetail;
                response.IsSuccess = true;
                response.StatusCode = StausCodes.Accepted;
                var skills = context.EmployeeSkills.Where(sk => sk.EmployeeId == employeeDetail.EmployeeId && sk.IsActive == true)
                    .Select(empSkill => new EmployeeSkill
                    {
                        EmployeeId = empSkill.EmployeeId,
                        levelId = empSkill.levelId,
                        skillId = empSkill.skillId,
                        rateId = empSkill.rateId
                    }).ToList();
                if (skills.Any())
                    response.Data.skills = skills;
            }
            else
            {
                response.IsSuccess = false;
                response.StatusCode = StausCodes.NotFound;
            }

            return response;
        }
    }
}
