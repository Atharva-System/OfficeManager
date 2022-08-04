using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.ApplicationUsers.Commands.RegisterApplicationUser;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Employees.Queries.GetAllEmployees;

namespace OfficeManager.API.Controllers
{
    //[Authorize]
    public class EmployeeController : ApiControllerBase
    {


        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<List<EmployeeDto>>> GetAll()
        {
            return await Mediator.Send(new GetAllEmployeesQuery());
        }
        private readonly IFilesServices _service;
        public EmployeeController(IFilesServices service)
        {
            _service = service;
        }
        [HttpGet]
        [Route("Upload")]
        public async Task<ActionResult<List<EmployeeDto>>> ReadFile(string path)
        {
            return Ok(await _service.ReadEmployeeExcel(path));
        }
    }
}
