using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.ApplicationRoles.Commands.CreateApplicationUserRoles;
using OfficeManager.Application.ApplicationRoles.Queries;
using OfficeManager.Application.Common.Models;

namespace OfficeManager.API.Controllers.Identity
{
    //[Authorize]
    [Route("api/Roles")]

    public class ApplicationUserRoleController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Result>> CreateApplicationRole(CreateApplicationUserRolesCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet]
        public async Task<ActionResult<List<ApplicationRolesDto>>> GetAll()
        {
            return await Mediator.Send(new GetApplicationRolesQuery());
        }
    }
}
