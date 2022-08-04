using Dapper;
using MediatR;
using OfficeManager.Application.Common.Interfaces;
using System.Data.SqlClient;

namespace OfficeManager.Application.Employees.Queries.GetAllEmployees
{
    public record GetAllEmployeesQuery : IRequest<List<EmployeeDto>>
    {
        public string Search { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid DesignationId { get; set; }
        public string RoleId { get; set; }
    }

    public class GetAllEmployeeQueryHandler : IRequestHandler<GetAllEmployeesQuery,List<EmployeeDto>>
    {
        private readonly IApplicationDbContext _context;
        //private readonly IContextServices _contextService;

        public GetAllEmployeeQueryHandler(IApplicationDbContext context)
        {
            _context = context;
            //_contextService = contextServices;
        }

        public async Task<List<EmployeeDto>> Handle(GetAllEmployeesQuery request,CancellationToken cancellationToken)
        {
            //var result = await _userManager.Users.Join<ApplicationUser, DesignationMaster, Guid, EmployeeDto>(
            //                _context.DesignationMasters,
            //                u => u.DesignationId,
            //                d => d.Id,
            //                (u, d) => new EmployeeDto
            //                {
            //                    id = u.Id,
            //                    FirstName = u.FirstName,
            //                    LastName = u.LastName,
            //                    Email = u.Email,
            //                    Contact = u.PhoneNumber,
            //                    Username = u.UserName,
            //                    Designation = d.Name,
            //                    Department = String.Empty,
            //                    Role = String.Empty
            //                })
            //                .ToListAsync();

            //var result = await _context.Users.Include("Designation")
            //    .Select(u => new EmployeeDto
            //    {
            //        id = u.Id,
            //        FirstName = u.FirstName,
            //        LastName = u.LastName,
            //        Email = u.Email,
            //        Contact = u.PhoneNumber,
            //        Username = u.UserName,
            //        Designation = u.Designation.Name,
            //        Department = String.Empty,
            //        Role = String.Empty
            //    })
            //    .ToListAsync();

            //SqlConnection connection = new SqlConnection(await _contextService.GetConnectionString());
            //var result = await connection.QueryAsync<EmployeeDto>("spGetAllEmp");
            //return result.ToList();

            return null;
        }
    }
}
