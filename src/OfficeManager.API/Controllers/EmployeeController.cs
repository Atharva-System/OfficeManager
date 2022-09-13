using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeManager.Application.Common.Constant;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Feature.Departments.Queries;
using OfficeManager.Application.Feature.Designations.Queries;
using OfficeManager.Application.Feature.Employees.Commands;
using OfficeManager.Application.Feature.Employees.Queries;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;
using System.Net.Http.Headers;

namespace OfficeManager.API.Controllers
{
    //[Authorize]
    public class EmployeeController : ApiControllerBase
    {
        private readonly IConfiguration Configuration;

        private readonly IFilesServices Service;
        public EmployeeController(IFilesServices service, IConfiguration configuration)
        {
            Service = service;
            Configuration = configuration;
        }
        //return paginated result useful for search and listing features
        [HttpGet]
        [Route("")]
        public async Task<IResponse> GetAll([FromQuery] GetAllEmployees query)
        
        {
            return (await Mediator.Send(query));
        }

        [HttpGet]
        [Route("{id}")]
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
        [Route("Import")]
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
                path = Configuration.GetValue<string>("ImportFile");
            var employees = await Service.ReadEmployeeExcel(path);
            var departments = await Mediator.Send(new GetAllDepartments());
            var designations = await Mediator.Send(new GetAllDesignations());
            AddBulkEmployees command = new AddBulkEmployees();
            command.employees = employees;
            command.departments = JsonConvert.DeserializeObject<DataResponse<List<DepartmentDTO>>>(JsonConvert.SerializeObject(departments))?.Data;
            command.designations = designations.Data;
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpPost]
        [Route("SaveBulk")]
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
        [Route("Edit")]
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
                response.StatusCode = StausCodes.InternalServerError;
                return StatusCode(500, response);
            }
        }

        [HttpPost]
        [Route("Add")]
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
                response.StatusCode = StausCodes.NotFound;
                response.IsSuccess = false;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
                response.IsSuccess = false;
                response.StatusCode = StausCodes.InternalServerError;
                return StatusCode(500, response);
            }
        }
    }
}
