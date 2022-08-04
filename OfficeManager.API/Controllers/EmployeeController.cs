using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.ApplicationUsers.Commands.RegisterApplicationUser;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Employees.Queries.GetAllEmployees;

namespace OfficeManager.API.Controllers
{
    [Authorize]
    public class EmployeeController : ApiControllerBase
    {


        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<List<EmployeeDto>>> GetAll()
        {
            return await Mediator.Send(new GetAllEmployeesQuery());
        }
    }
}
