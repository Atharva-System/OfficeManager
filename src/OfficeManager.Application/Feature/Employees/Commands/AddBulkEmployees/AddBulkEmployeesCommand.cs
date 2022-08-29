using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Employees.Commands.AddBulkEmployees
{
    public record AddBulkEmployeesCommand : IRequest<Response<List<BulkImportEmployeeDTO>>>
    {
        public List<BulkImportEmployeeDTO> employees { get; set; }
        public List<DepartmentDTO> departments { get; set; }
        public List<DesignationDTO> designations { get; set; }

        public AddBulkEmployeesCommand()
        {
            employees = new List<BulkImportEmployeeDTO>();

            departments = new List<DepartmentDTO>();

            designations = new List<DesignationDTO>();
        }

    }
    public class AddBulkEmployeesCommandHandler : IRequestHandler<AddBulkEmployeesCommand, Response<List<BulkImportEmployeeDTO>>>
    {
        private readonly IApplicationDbContext context;
        public AddBulkEmployeesCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Response<List<BulkImportEmployeeDTO>>> Handle(AddBulkEmployeesCommand request, CancellationToken cancellationToken)
        {
            Response<List<BulkImportEmployeeDTO>> response = new Response<List<BulkImportEmployeeDTO>>();
            try
            {
                RoleMaster role = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
                if (((request.departments != null && request.departments.Count > 0)
                   || (request.designations != null && request.designations.Count > 0)) && request.employees.Count > 0)
                {
                    request.employees.ForEach(async emp =>
                    {
                        var department = request.departments.FirstOrDefault(dept => dept.Name.Replace(" ","").ToLower().Trim().Equals(emp.Department.Replace(" ", "").ToLower().Trim()));
                        var designation = request.designations.FirstOrDefault(des => des.Name.Replace(" ", "").ToLower().Trim().Equals(emp.Designation.Replace(" ", "").ToLower().Trim()));
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
                        emp.RoleId = role != null ? role.Id : context.Roles.FirstOrDefault().Id;
                    });
                    response.Data = request.employees;
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
                response.Data = new List<BulkImportEmployeeDTO>();
                response._Message = "There is some error in data";
                response._Errors.Add(ex.Message);
                response._IsSuccess = false;
                response._StatusCode = "500";
                return response;
            }
        }
    }
}
