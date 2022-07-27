using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Designations.Commands.CreateDesignationCommand;
using OfficeManager.Application.Designations.Commands.DeleteDesignationCommand;
using OfficeManager.Application.Designations.Queries.GetAllDesignationsQuery;

namespace OfficeManager.API.Controllers
{
    //[Authorize]
    public class DesignationController : ApiControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<DesignationDto>>> GetAll(string? search)
        {
            return await Mediator.Send(new GetAllDesignationQuery(search));
        }

        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult<Result>> CreateDesignation(CreateDesignationCommand command)
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

        [HttpDelete]
        [Route("{id}/Delete")]
        public async Task<ActionResult<Result>> DeleteDesignation(Guid id)
        {
            try
            {
                var result = await Mediator.Send(new DeleteDesignationCommand(id));
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
