using MediatR;

namespace OfficeManager.Application.Employees.Queries.GetAllEmployees
{
    public record GetAllEmployeesQuery : IRequest<List<EmployeeDto>>
    {
        public string Search { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid DesignationId { get; set; }
        public string RoleId { get; set; }
    }

    public class GetAllEmployeeQueryHandler //: IRequestHandler<GetAllEmployeesQuery,List<EmployeeDto>>
    {
        //private
    }
}
