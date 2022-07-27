using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Departments.Commands.CreateDepartmentCommand;
using OfficeManager.Application.Departments.Commands.DeleteDepartmentCommand;
using OfficeManager.Application.Departments.Commands.UpdateDepartment;
using OfficeManager.Application.Departments.Queries.GetAllDepartmentsQuery;

namespace OfficeManager.API.Controllers
{
    //[Authorize]
    public class DepartmentController : ApiControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<List<DepartmentDto>> GetAll(string? search)
        {
            return await Mediator.Send(new GetAllDepartmentsQuery(search));
        }

        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult<Result>> CreateDepartment(CreateDepartmentCommand command)
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
        public async Task<ActionResult<Result>> DeleteDepartment(Guid id)
        {
            try
            {
                var result = await Mediator.Send(new DeleteDepartmentCommand(id));
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

        [HttpPut]
        [Route("Edit")]
        public async Task<ActionResult<Result>> EditDepartment(UpdateDepartmentCommand command)
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
    }
}
