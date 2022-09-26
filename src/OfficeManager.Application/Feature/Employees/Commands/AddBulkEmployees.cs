using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Exceptions;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;
using OfficeManager.Domain.Entities;
using System.Net.Http.Headers;

namespace OfficeManager.Application.Feature.Employees.Commands
{
    public record AddBulkEmployees : IRequest<IResponse>
    {
        public List<IFormFile> files { get; set; }
        public string path { get; set; }
        public List<DepartmentDTO> departments { get; set; }
        public List<DesignationDTO> designations { get; set; }

        public AddBulkEmployees()
        {
            departments = new List<DepartmentDTO>();
            designations = new List<DesignationDTO>();
        }

    }
    public class AddBulkEmployeesCommandHandler : IRequestHandler<AddBulkEmployees, IResponse>
    {
        private readonly IApplicationDbContext Context;
        private readonly IFilesServices service;
        public AddBulkEmployeesCommandHandler(IApplicationDbContext context, IFilesServices service)
        {
            Context = context;
            this.service = service;
        }

        public async Task<IResponse> Handle(AddBulkEmployees request, CancellationToken cancellationToken)
        {
            try
            {
                long size = request.files.Sum(f => f.Length);
                var folderName = Path.Combine("Resources");
                string path = "";
                foreach (var file in request.files)
                {
                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(request.path, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        path = fullPath;
                    }
                }

                var employees = await service.ReadEmployeeExcel(path);

                RoleMaster role = await Context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");

                BulkImportEmployeeValidInvalidListDTO data = new BulkImportEmployeeValidInvalidListDTO();

                if ((request.departments != null && request.departments.Count > 0
                   || request.designations != null && request.designations.Count > 0) && employees.Count > 0)
                {
                    employees.ForEach(async emp =>
                    {
                        var department = request.departments.FirstOrDefault(dept => dept.Name.Replace(" ", "").ToLower().Trim().Equals(emp.Department.Replace(" ", "").ToLower().Trim()));
                        var designation = request.designations.FirstOrDefault(des => des.Name.Replace(" ", "").ToLower().Trim().Equals(emp.Designation.Replace(" ", "").ToLower().Trim()));
                        var employee = Context.Employees.FirstOrDefault(emp2 => emp2.EmployeeNo == emp.EmployeeNo);


                        emp.RoleId = role != null ? role.Id : Context.Roles.FirstOrDefault().Id;

                        if (department == null || designation == null)
                        {
                            emp.IsValid = false;
                            if (department == null)
                            {
                                emp.ValidationErros.Add("Department is missing!");
                            }
                            if (designation == null)
                            {
                                emp.ValidationErros.Add("Designtion is missing!");
                            }
                            if (employee != null)
                            {
                                emp.Id = employee.Id;
                            }
                            data.InvalidImportEmployeeList.Add(emp);
                        }
                        else
                        {
                            if (department != null)
                            {
                                emp.DepartmentId = department.Id;
                            }
                            if (designation != null)
                            {
                                emp.DesignationId = designation.Id;
                            }

                            if (employee != null)
                            {
                                emp.Id = employee.Id;
                                data.UpdateEmployeeList.Add(emp);
                            }
                            else
                            {
                                data.NewEmployeeList.Add(emp);
                            }
                        }

                    });
                    return new DataResponse<BulkImportEmployeeValidInvalidListDTO>(data, StatusCodes.Accepted, Messages.DataFound);
                }
                return new ErrorResponse(StatusCodes.BadRequest, "Uploaded sheet has some issue please check");
            }
            catch(ValidationException exception)
            {
                throw exception;
            }
            catch(ForbiddenAccessException exception)
            {
                throw exception;
            }
            catch(NotFoundException exception)
            {
                throw exception;
            }
            catch(Exception ex)
            {
                return new ErrorResponse(StatusCodes.InternalServerError, ex.Message);
            }
        }
    }
}
