using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.ApplicationRoles.Commands.CreateApplicationUserRoles;
using OfficeManager.Application.ApplicationRoles.Commands.DeleteUserRoles;
using OfficeManager.Application.ApplicationRoles.Queries;
using OfficeManager.Application.Common.Models;

namespace OfficeManager.API.Controllers.Identity
{
    //[Authorize]
    [Route("api/UserRoles")]

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

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult<Result>> DeleteApplicationRole(string id)
        {
            return await Mediator.Send(new DeleteUserRoleCommand(id));
        }
    }
}
