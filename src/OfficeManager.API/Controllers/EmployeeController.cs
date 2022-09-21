using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Feature.Departments.Queries;
using OfficeManager.Application.Feature.Designations.Queries;
using OfficeManager.Application.Feature.Employees.Commands;
using OfficeManager.Application.Feature.Employees.Queries;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;

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
        public async Task<IResponse> GetEmployeeDetail(int id)
        {
            return await Mediator.Send(new GetEmployeeDetail(id));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Import")]
        public async Task<IResponse> UploadBulkEmployeeImportData(List<IFormFile> file)
        {
            var folderName = Path.Combine("Resources");
            var departments = await Mediator.Send(new GetAllDepartments());
            var designations = await Mediator.Send(new GetAllDesignations());
            AddBulkEmployees command = new AddBulkEmployees();
            command.files = file;
            command.path = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            command.departments = JsonConvert.DeserializeObject<DataResponse<List<DepartmentDTO>>>(JsonConvert.SerializeObject(departments))?.Data;
            command.designations = JsonConvert.DeserializeObject<DataResponse<List<DesignationDTO>>>(JsonConvert.SerializeObject(designations))?.Data;
            return await Mediator.Send(command);
        }

        [HttpPost]
        [Route("SaveBulk")]
        public async Task<IResponse> SaveBulkEmployees([FromBody] SaveBulkEmployees command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut]
        [Route("Edit")]
        public async Task<IResponse> UpdateEmployee([FromBody] UpdateEmployee command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IResponse> AddEmployee([FromBody] AddEmployee command)
        {
            return await Mediator.Send(command);
        }
    }
}
