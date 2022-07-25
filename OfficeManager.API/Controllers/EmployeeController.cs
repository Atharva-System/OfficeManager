using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.ApplicationUsers.Commands.RegisterApplicationUser;
using OfficeManager.Application.Common.Models;

namespace OfficeManager.API.Controllers
{
    [Authorize]
    public class EmployeeController : ApiControllerBase
    {
        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult<Result>> AddEmployee(RegisterApplicationUserCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
