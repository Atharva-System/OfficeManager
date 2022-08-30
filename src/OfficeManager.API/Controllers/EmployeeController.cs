using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Feature.Departments.Queries;
using OfficeManager.Application.Feature.Designations.Queries;
using OfficeManager.Application.Feature.Employees.Commands;
using OfficeManager.Application.Feature.Employees.Queries;
using System.Net.Http.Headers;

namespace OfficeManager.API.Controllers
{
    [Authorize]
    public class EmployeeController : ApiControllerBase
    {
        private readonly IConfiguration configuration;

        [HttpGet]
        [Route("GetAllEmployee")]
        public async Task<ActionResult<Response<EmployeeListResponse>>> GetAll(string? search, int? DepartmentId,int? DesignationId,int? RoleId,string? DOBFrom, string? DOBTo, string? DOJFrom, string? DOJTo
            ,int PageNo,int PageSize)
        
        {
            var DobFrom = DateTime.Parse(String.IsNullOrEmpty(DOBFrom)? "01/01/1753" : DOBFrom);
            var DobTo = DateTime.Parse(String.IsNullOrEmpty(DOBTo)? "12/12/9999" : DOBTo);
            var DojFrom = DateTime.Parse(String.IsNullOrEmpty(DOJFrom)? "01/01/1753" : DOJFrom);
            var DojTo = DateTime.Parse(String.IsNullOrEmpty(DOJTo)? "12/12/9999" : DOJTo);
            GetAllEmployees query = new GetAllEmployees(search,(DepartmentId != null?DepartmentId.Value:0), (DesignationId != null ? DesignationId.Value : 0), RoleId,DobFrom,DobTo,DojFrom,DojTo,PageNo,PageSize);
            var response = await Mediator.Send(query);
            if (response.StatusCode == "500")
                return StatusCode(500, response);
            else if (response.StatusCode == "404")
                return NotFound(response);
            else
                return Ok(response);
        }
        private readonly IFilesServices service;
        public EmployeeController(IFilesServices service, IConfiguration configuration)
        {
            this.service = service;
            this.configuration = configuration;
        }



        [HttpGet]
        [Route("GetEmployeeById/{id}")]
        public async Task<ActionResult<Response<EmployeeDetailDTO>>> GetEmployeeDetail(int id)
        {
            try
            {
                var result = await Mediator.Send(new GetEmployeeDetail(id));
                if (result.Data == null)
                    NotFound(result);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Search has some issue please check internet connection.");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("UploadBulkEmployeeImportData")]
        public async Task<ActionResult<Response<List<BulkImportEmployeeDTO>>>> UploadBulkEmployeeImportData(List<IFormFile> file)
        {
            long size = file.Sum(f => f.Length);
            var folderName = Path.Combine("Resources");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            string path = "";
            foreach (var formFile in file)
            {
                if (formFile.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        formFile.CopyTo(stream);
                    }
                    path = fullPath;
                }
            }
            if (string.IsNullOrEmpty(path))
                path = configuration.GetValue<string>("ImportFile");
            var employees = await service.ReadEmployeeExcel(path);
            var departments = await Mediator.Send(new SearchDepartments(null));
            var designations = await Mediator.Send(new SearchDesignations(null));
            AddBulkEmployees command = new AddBulkEmployees();
            command.employees = employees;
            command.departments = departments.Data;
            command.designations = designations.Data;
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpPost]
        [Route("SaveBulkEmployees")]
        public async Task<ActionResult<Response<object>>> SaveBulkEmployees([FromBody] SaveBulkEmployees command)
        {
            try
            {
                var response = await Mediator.Send(command);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Some issue with data");
            }
        }

        [HttpPut]
        [Route("EditEmployee")]
        public async Task<ActionResult<Response<object>>> UpdateEmployee([FromBody] UpdateEmployee command)
        {
            Response<object> response = new Response<object>();
            try
            {
                response = await Mediator.Send(command);
                if (response.IsSuccess)
                    return Ok(response);
                return BadRequest(response);
            }
            catch(ValidationException ex)
            {
                response.Errors = ex.Errors.Select(err => err.ErrorMessage).ToList();
                response.StatusCode = "400";
                response.IsSuccess = false;
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                response.Errors.Add(ex.Message);
                response.IsSuccess = false;
                response.StatusCode = "500";
                return StatusCode(500, response);
            }
        }

        [HttpPost]
        [Route("AddEmployee")]
        public async Task<ActionResult<Response<object>>> AddEmployee([FromBody] AddEmployee command)
        {
            Response<object> response = new Response<object>();
            try
            {
                response = await Mediator.Send(command);
                if (response.IsSuccess)
                    return Ok(response);
                return BadRequest(response);
            }
            catch (ValidationException ex)
            {
                response.Errors = ex.Errors.Select(err => err.ErrorMessage).ToList();
                response.StatusCode = "400";
                response.IsSuccess = false;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
                response.IsSuccess = false;
                response.StatusCode = "500";
                return StatusCode(500, response);
            }
        }
    }
}
