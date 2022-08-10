using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Departments.Queries.SearchDepartments;
using OfficeManager.Application.Designations.Queries.SearchDesignationsQuery;

namespace OfficeManager.Application.Employees.Commands.AddBulkEmployees
{
    public record AddBulkEmployeesCommand : IRequest<Response<List<BIEmployeeDto>>>
    {
        public List<BIEmployeeDto> _employees { get; set; }
        public List<DepartmentDto> _departments { get; set; }
        public List<DesignationDto> _designations { get; set; }

    }
    public class AddBulkEmployeesCommandHandler : IRequestHandler<AddBulkEmployeesCommand, Response<List<BIEmployeeDto>>>
    {
        private readonly IApplicationDbContext _context;
        public AddBulkEmployeesCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<List<BIEmployeeDto>>> Handle(AddBulkEmployeesCommand request, CancellationToken cancellationToken)
        {
            Response<List<BIEmployeeDto>> response = new Response<List<BIEmployeeDto>>();
            try
            {
                if (((request._departments != null && request._departments.Count > 0)
                   || (request._designations != null && request._designations.Count > 0)) && request._employees.Count > 0)
                {
                    request._employees.ForEach(async emp =>
                    {
                        var department = request._departments.FirstOrDefault(dept => dept.Name.Replace(" ","").ToLower().Trim().Equals(emp.Department.Replace(" ", "").ToLower().Trim()));
                        var designation = request._designations.FirstOrDefault(des => des.Name.Replace(" ", "").ToLower().Trim().Equals(emp.Designation.Replace(" ", "").ToLower().Trim()));
                        if (department != null)
                        {
                            emp.DepartmentId = department.Id;
                        }
                        else
                        {
                            emp.IsValid = false;
                            emp.ValidationErros.Add("Department is missing!");
                        }

                        if (designation != null)
                        {
                            emp.DesignationId = designation.Id;
                        }
                        else
                        {
                            emp.IsValid = false;
                            emp.ValidationErros.Add("Designtion is missing!");
                        }
                        emp.RoleId = 1;
                    });
                    response.Data = request._employees;
                    response._Message = "Employees bulk insertion set to verify!";
                    response._IsSuccess = true;
                    response._StatusCode = "200";
                }
                else
                {
                    response._Message = "Data not found";
                    response._IsSuccess = false;
                    response._StatusCode = "404";
                }
                return response;
            }
            catch(Exception ex)
            {
                response.Data = null;
                response._Message = "There is some error in data";
                response._Errors.Add(ex.Message);
                response._IsSuccess = false;
                response._StatusCode = "500";
                return response;
            }
        }
    }
}
