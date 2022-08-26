using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Skills.Commands.CreateSkill;
using OfficeManager.Application.Skills.Queries.SearchSkillsQuery;

namespace OfficeManager.API.Controllers
{
    [Authorize]
    public class SkillController : ApiControllerBase
    {
        [HttpGet]
        [Route("GetAllSkill")]
        public async Task<ActionResult<Response<PaginatedList<SkillDto>>>> GetSkills([FromQuery] SearchSkillsQuery query)
        {
            Response<PaginatedList<SkillDto>> response = new Response<PaginatedList<SkillDto>>();
            try
            {
                response = await Mediator.Send(query);
                if (response._IsSuccess)
                    return Ok(response);
                return NotFound(response);
            }
            catch (ValidationException ex)
            {
                response._Errors = ex.Errors.Select(err => err.ErrorMessage).ToList();
                response._Message = "Validation Failed";
                response._IsSuccess = false;
                response._StatusCode = "400";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response._Errors.Add(ex.Message);
                response._Message = "Internal Server Error";
                response._IsSuccess = false;
                response._StatusCode = "500";
                return StatusCode(500, response);
            }
        }

        [HttpPost]
        [Route("AddSkill")]
        public async Task<ActionResult<Response<object>>> AddSkill(CreateSkillCommand command)
        {
            Response<object> response = new Response<object>();
            try
            {
                response = await Mediator.Send(command);
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                response._Errors = ex.Errors.Select(err => err.ErrorMessage).ToList();
                response._Message = "Validation Failed";
                response._IsSuccess = false;
                response._StatusCode = "400";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response._Errors.Add(ex.Message);
                response._Message = "Internal Server Error";
                response._IsSuccess = false;
                response._StatusCode = "500";
                return StatusCode(500, response);
            }
        }
    }
}