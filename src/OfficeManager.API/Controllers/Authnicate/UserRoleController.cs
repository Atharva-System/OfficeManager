using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.ApplicationRoles.Commands.CreateApplicationUserRoles;
using OfficeManager.Application.ApplicationRoles.Commands.DeleteUserRoles;
using OfficeManager.Application.ApplicationRoles.Queries;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Models;

namespace OfficeManager.API.Controllers.Identity
{
    [Authorize]
    [Route("api/UserRoles")]

    public class UserRoleController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Result>> CreateApplicationRole(CreateUserRolesCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                if (result.Succeeded)
                    return Ok(result);
                return BadRequest(result);
            }catch(ValidationException ex)
            {
                return BadRequest(Result.Failure(ex.Errors.Select(x => x.ErrorMessage).ToList(),ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(Result.Failure(Enumerable.Empty<string>(), ex.Message));
            }
        }

        //[Authorize]
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<List<RolesDTO>>> GetAll()
        {
            return await Mediator.Send(new GetUserRolesQuery());
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult<Result>> DeleteUserRole(int id)
        {
            try
            {
                var result = await Mediator.Send(new DeleteUserRoleCommand(id));
                if (result.Succeeded)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(Result.Failure(ex.Errors.Select(x => x.ErrorMessage).ToList(), ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(Result.Failure(Enumerable.Empty<string>(), ex.Message));
            }
        }
    }
}
