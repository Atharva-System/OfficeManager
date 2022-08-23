using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Departments.Queries.SearchDepartments;
using OfficeManager.Application.Designations.Queries.SearchDesignationsQuery;
using OfficeManager.Application.Employees.Commands.AddBulkEmployees;
using OfficeManager.Application.Employees.Commands.AddEmployee;
using OfficeManager.Application.Employees.Commands.SaveBulkEmployees;
using OfficeManager.Application.Employees.Commands.UpdateEmployee;
using OfficeManager.Application.Employees.Queries.GetAllEmployees;
using OfficeManager.Application.Employees.Queries.GetEmployeeById;
using System.Net.Http.Headers;

namespace OfficeManager.API.Controllers
{
    [Authorize]
    public class EmployeeController : ApiControllerBase
    {
        private readonly IConfiguration _configuration;

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<Response<EmployeeListResponse>>> GetAll(string? search, int? DepartmentId,int? DesignationId,int? RoleId,string? DOBFrom, string? DOBTo, string? DOJFrom, string? DOJTo
            ,int PageNo,int PageSize)
        
        {
            var DobFrom = DateTime.Parse(String.IsNullOrEmpty(DOBFrom)? "01/01/1753" : DOBFrom);
            var DobTo = DateTime.Parse(String.IsNullOrEmpty(DOBTo)? "12/12/9999" : DOBTo);
            var DojFrom = DateTime.Parse(String.IsNullOrEmpty(DOJFrom)? "01/01/1753" : DOJFrom);
            var DojTo = DateTime.Parse(String.IsNullOrEmpty(DOJTo)? "12/12/9999" : DOJTo);
            GetAllEmployeesQuery query = new GetAllEmployeesQuery(search,(DepartmentId != null?DepartmentId.Value:0), (DesignationId != null ? DesignationId.Value : 0), RoleId,DobFrom,DobTo,DojFrom,DojTo,PageNo,PageSize);
            var response = await Mediator.Send(query);
            if (response._StatusCode == "500")
                return StatusCode(500, response);
            else if (response._StatusCode == "404")
                return NotFound(response);
            else
                return Ok(response);
        }
        private readonly IFilesServices _service;
        public EmployeeController(IFilesServices service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }



        [HttpGet]
        [Route("Detail/{id}")]
        public async Task<ActionResult<Response<EmployeeDetailDto>>> GetEmployeeDetail(int id)
        {
            try
            {
                var result = await Mediator.Send(new GetEmployeeDetailQuery(id));
                if (result._Data == null)
                    NotFound(result);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Search has some issue please check internet connection.");
            }
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<ActionResult<Response<List<BIEmployeeDto>>>> ReadFile(List<IFormFile> file)
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
                path = _configuration.GetValue<string>("ImportFile");
            var employees = await _service.ReadEmployeeExcel(path);
            var departments = await Mediator.Send(new SearchDepartmentsQuery(null));
            var designations = await Mediator.Send(new SearchDesignationsQuery(null));
            AddBulkEmployeesCommand command = new AddBulkEmployeesCommand();
            command._employees = employees;
            command._departments = departments.Data;
            command._designations = designations.Data;
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpPost]
        [Route("BulkAdd")]
        public async Task<ActionResult<Response<object>>> AddBulkEmployee([FromBody] SaveBulkEmployeesCommand command)
        {
            try
            {
                var response = await Mediator.Send(command);
                if (response._IsSuccess)
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
        [Route("Edit")]
        public async Task<ActionResult<Response<object>>> UpdateEmployee([FromBody] UpdateEmployeeCommand command)
        {
            Response<object> response = new Response<object>();
            try
            {
                response = await Mediator.Send(command);
                if (response._IsSuccess)
                    return Ok(response);
                return BadRequest(response);
            }
            catch(ValidationException ex)
            {
                response._Errors = ex.Errors.Select(err => err.ErrorMessage).ToList();
                response._StatusCode = "400";
                response._IsSuccess = false;
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                response._Errors.Add(ex.Message);
                response._IsSuccess = false;
                response._StatusCode = "500";
                return StatusCode(500, response);
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<Response<object>>> AddEmployee([FromBody] AddEmployeeCommand command)
        {
            Response<object> response = new Response<object>();
            try
            {
                response = await Mediator.Send(command);
                if (response._IsSuccess)
                    return Ok(response);
                return BadRequest(response);
            }
            catch (ValidationException ex)
            {
                response._Errors = ex.Errors.Select(err => err.ErrorMessage).ToList();
                response._StatusCode = "400";
                response._IsSuccess = false;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response._Errors.Add(ex.Message);
                response._IsSuccess = false;
                response._StatusCode = "500";
                return StatusCode(500, response);
            }
        }
    }
}
