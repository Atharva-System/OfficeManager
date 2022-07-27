using FluentValidation;
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

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<List<ApplicationRolesDto>>> GetAll()
        {
            return await Mediator.Send(new GetApplicationRolesQuery());
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult<Result>> DeleteApplicationRole(string id)
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
