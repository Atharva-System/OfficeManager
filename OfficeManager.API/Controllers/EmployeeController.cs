using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Departments.Queries.SearchDepartments;
using OfficeManager.Application.Designations.Queries.SearchDesignationsQuery;
using OfficeManager.Application.Employees.Commands.AddBulkEmployees;
using OfficeManager.Application.Employees.Commands.SaveBulkEmployees;
using OfficeManager.Application.Employees.Queries.GetAllEmployees;
using System.Net.Http.Headers;

namespace OfficeManager.API.Controllers
{
    //[Authorize]
    public class EmployeeController : ApiControllerBase
    {
        private readonly IConfiguration _configuration;

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<List<EmployeeDto>>> GetAll()
        {
            return await Mediator.Send(new GetAllEmployeesQuery());
        }
        private readonly IFilesServices _service;
        public EmployeeController(IFilesServices service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
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
    }
}
