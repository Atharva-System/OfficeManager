using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Common.Constant;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Feature.Skills.Commands;
using OfficeManager.Application.Feature.Skills.Queries;

namespace OfficeManager.API.Controllers
{
    [Authorize]
    public class SkillController : ApiControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<Response<PaginatedList<SkillDTO>>>> GetSkills([FromQuery] SearchSkills query)
        {
            Response<PaginatedList<SkillDTO>> response = new Response<PaginatedList<SkillDTO>>();
            try
            {
                response = await Mediator.Send(query);
                if (response.IsSuccess)
                    return Ok(response);
                return NotFound(response);
            }
            catch (ValidationException ex)
            {
                response.Errors = ex.Errors.Select(err => err.ErrorMessage).ToList();
                response.Message = "Validation Failed";
                response.IsSuccess = false;
                response.StatusCode = "400";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
                response.Message = "Internal Server Error";
                response.IsSuccess = false;
                response.StatusCode = StausCodes.InternalServerError;
                return StatusCode(500, response);
            }
        }

        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult<Response<object>>> AddSkill(CreateSkill command)
        {
            Response<object> response = new Response<object>();
            try
            {
                response = await Mediator.Send(command);
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                response.Errors = ex.Errors.Select(err => err.ErrorMessage).ToList();
                response.Message = "Validation Failed";
                response.IsSuccess = false;
                response.StatusCode = "400";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
                response.Message = "Internal Server Error";
                response.IsSuccess = false;
                response.StatusCode = StausCodes.InternalServerError;
                return StatusCode(500, response);
            }
        }
    }
}