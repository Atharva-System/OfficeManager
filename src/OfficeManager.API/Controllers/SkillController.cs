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
        //return paginated result useful for search and listing features
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<Response<PaginatedList<SkillDTO>>>> Search([FromQuery] SearchSkills query)
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
        //return all the deaprtments usefull for dropdown like usage
        [HttpGet]
        [Route("All")]
        public async Task<ActionResult<Response<List<SkillDTO>>>> GetAll()
        {
            var response = await Mediator.Send(new GetAllSkills());
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