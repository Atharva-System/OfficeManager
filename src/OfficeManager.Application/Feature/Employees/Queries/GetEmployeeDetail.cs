using MediatR;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;

namespace OfficeManager.Application.Feature.Employees.Queries
{
    public record GetEmployeeDetail(int employeeId) : IRequest<IResponse>;

    public class GetEmployeeDetailQueryHandler : IRequestHandler<GetEmployeeDetail, IResponse>
    {
        private readonly IApplicationDbContext Context;
        public GetEmployeeDetailQueryHandler(IApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<IResponse> Handle(GetEmployeeDetail request, CancellationToken cancellationToken)
        {
            Response<EmployeeDetailDTO> response = new Response<EmployeeDetailDTO>();
            response.Data = new EmployeeDetailDTO();

            var employeeDetail = Context.Employees.Where(emp => emp.Id == request.employeeId)
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
                    UserId = Context.Users.FirstOrDefault(u => u.EmployeeID == emp.Id).Id
                }).FirstOrDefault();

            if (employeeDetail != null)
            {
                employeeDetail.RoleId = Context.UserRoleMapping.FirstOrDefault(ur => ur.UserId == employeeDetail.UserId).RoleId;
                response.Data = employeeDetail;
                var skills = Context.EmployeeSkills.Where(sk => sk.EmployeeId == employeeDetail.EmployeeId && sk.IsActive == true)
                    .Select(empSkill => new EmployeeSkill
                    {
                        EmployeeId = empSkill.EmployeeId,
                        levelId = empSkill.levelId,
                        skillId = empSkill.skillId,
                        rateId = empSkill.rateId
                    }).ToList();
                if (skills.Any())
                    response.Data.skills = skills;
                return new DataResponse<EmployeeDetailDTO>(employeeDetail, StatusCodes.Accepted, Messages.DataFound);
            }

            return new ErrorResponse(StatusCodes.NotFound,Messages.NotFound);
        }
    }
}
