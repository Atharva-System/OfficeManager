using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.API.Filters;
using OfficeManager.Application.ApplicationRoles.Queries;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Feature.UserRoles.Commands;
using OfficeManager.Application.Feature.UserRoles.Queries;

namespace OfficeManager.API.Controllers
{
    [Authorize]
    [Route("api/v1/UserRoles")]
    public class UserRoleController : ApiControllerBase
    {
        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult<Result>> CreateApplicationRole(CreateUserRoles command)
        {
            try
            {
                var result = await Mediator.Send(command);
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
        //return all the deaprtments usefull for dropdown like usage
        //[Authorize]
        [HttpGet]
        [Route("All")]
        public async Task<ActionResult<Response<List<RolesDTO>>>> GetAll()
        {
            var response = await Mediator.Send(new GetUserRoles());
            if (response.StatusCode == StatusCodes.Status500InternalServerError.ToString())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response);
        }

        //return paginated result useful for search and listing features
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<Response<PaginatedList<RolesDTO>>>> Search([FromQuery] SearchUserRoles query)
        {
            var response = await Mediator.Send(query);
            if (response.StatusCode == StatusCodes.Status400BadRequest.ToString())
            {
                return BadRequest(response);
            }
            else if (response.StatusCode == StatusCodes.Status500InternalServerError.ToString())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult<Result>> DeleteUserRole(int id)
        {
            try
            {
                var result = await Mediator.Send(new DeleteUserRole(id));
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
