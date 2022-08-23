using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Departments.Queries.SearchDepartments;
using OfficeManager.Application.Designations.Queries.SearchDesignationsQuery;
using OfficeManager.Application.Employees.Queries.GetEmployeeById;
using OfficeManager.Application.Skills.Commands.CreateSkill;
using OfficeManager.Application.Skills.Queries.GetAllSkillLevels;
using OfficeManager.Application.Skills.Queries.GetAllSkillRates;
using OfficeManager.Application.Skills.Queries.SearchSkillsQuery;
using OfficeManager.Domain.Entities;

namespace OfficeManager.API.Controllers
{
    [Authorize]
    public class MastersController : ApiControllerBase
    {
        [HttpGet]
        [Route("Departments")]
        public async Task<ActionResult<Response<List<DepartmentDto>>>> SearchDepartment(string? search)
        {
            try
            {
                var result = await Mediator.Send(new SearchDepartmentsQuery(search));
                if (result.Data == null)
                    NotFound("No records found");
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest("Search has some issue please check internet connection.");
            }
        }
        [HttpGet]
        [Route("Designations")]
        public async Task<ActionResult<Response<List<DesignationDto>>>> SearchDesignation(string? search)
        {
            try
            {
                var result = await Mediator.Send(new SearchDesignationsQuery(search));
                if (result.Data == null)
                    NotFound("No records found");
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest("Search has some issue please check internet connection.");
            }
        }
        [HttpPost]
        [Route("Skill/Add")]
        public async Task<ActionResult<Response<object>>> AddSkill(CreateSkillCommand command)
        {
            Response<object> response = new Response<object>();
            try
            {
                response = await Mediator.Send(command);
                return Ok(response);
            }
            catch(ValidationException ex)
            {
                response._Errors = ex.Errors.Select(err => err.ErrorMessage).ToList();
                response._Message = "Validation Failed";
                response._IsSuccess = false;
                response._StatusCode = "400";
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                response._Errors.Add(ex.Message);
                response._Message = "Internal Server Error";
                response._IsSuccess = false;
                response._StatusCode = "500";
                return StatusCode(500, response);
            }
        }

        [HttpGet]
        [Route("SkillRates")]
        public async Task<ActionResult<Response<List<SkillRate>>>> GetAllSkillRates()
        {
            try
            {
                var result = await Mediator.Send(new GetAllSkillRatesQuery());
                if (result._StatusCode == "404")
                    return NotFound(result);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Please check data, it is internal server error.");
            }
        }

        [HttpGet]
        [Route("SkillLevels")]
        public async Task<ActionResult<Response<List<SkillLevel>>>> GetAllSkillLevels()
        {
            try
            {
                var result = await Mediator.Send(new GetAllSkillLevelsQuery());
                if (result._StatusCode == "404")
                    return NotFound(result);
                return Ok(result);
            }
            catch(Exception)
            {
                return StatusCode(500, "Please check data, it is internal server error.");
            }
        }

        [HttpGet]
        [Route("Skill")]
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
    }
}
