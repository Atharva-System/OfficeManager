using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.UserRoles.Command.CreateUserRoles;
using OfficeManager.Application.UserRoles.Command.DeleteUserRoles;
using OfficeManager.Application.UserRoles.Command.UpdateUserRoles;
using OfficeManager.Application.UserRoles.Queries.GetAllUserRolesQueries;

namespace OfficeManager.API.Controllers
{
    public class RolesController : ApiControllerBase
    {
        [Authorize]
        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult<Result>> AddRole([FromBody] CreateUserRoleCommand command)
        {
            return await Mediator.Send(command);
        }

        [Authorize]
        [HttpPut]
        [Route("Edit")]
        public async Task<ActionResult<Result>> EditRole([FromBody] UpdateUserRoleCommand command)
        {
            return await Mediator.Send(command);
        }

        [Authorize]
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult<Result>> DeleteRole(Guid id)
        {
            return await Mediator.Send(new DeleteUserRoleCommand(id));
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<UserRoleDto>>> GetAll(string? search)
        {
            return await Mediator.Send(new GetAllUserRolesQuery(search));
        }
    }
}
