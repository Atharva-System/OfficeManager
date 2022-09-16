using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Feature.Departments.Queries;
using OfficeManager.Application.Feature.Employees.Commands;
namespace OfficeManager.API.Controllers
{
    [Authorize]
    public class DepartmentController : ApiControllerBase
    {
        //return all the deaprtments usefull for dropdown like usage
        [HttpGet]
        [Route("All")]
        public async Task<ActionResult<Response<List<DepartmentDTO>>>> GetAll()
        {
            try
            {
                var result = await Mediator.Send(new GetAllDepartments());
                if (result.Data == null)
                    NotFound("No records found");
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest("Search has some issue please check internet connection.");
            }
        }

        //return paginated result useful for search and listing features
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<Response<PaginatedList<DepartmentDTO>>>> Search([FromQuery] SearchDepartments query)
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

        [HttpGet]
        [Route("Filter")]
        public async Task<ActionResult<Response<PaginatedList<DepartmentDTO>>>> Filter([FromQuery] FilterDepartments query)
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
        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult<Response<PaginatedList<DepartmentDTO>>>> AddDepartment([FromBody] AddDepartment command)
        {
            Response<object> response = new Response<object>();
            try
            {
                response = await Mediator.Send(command);
                if (response.IsSuccess)
                    return Ok(response);
                return BadRequest(response);
            }
            catch (ValidationException ex)
            {
                response.Errors = ex.Errors.Select(err => err.ErrorMessage).ToList();
                response.StatusCode = "400";
                response.IsSuccess = false;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
                response.IsSuccess = false;
                response.StatusCode = StatusCodes.Status500InternalServerError.ToString();
                return StatusCode(500, response);
            }
        }
        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult<Response<PaginatedList<DepartmentDTO>>>> UpdateDepartment([FromBody] UpdateDepartment command)
        {
            Response<object> response = new Response<object>();
            try
            {
                response = await Mediator.Send(command);
                if (response.IsSuccess)
                    return Ok(response);
                return BadRequest(response);
            }
            catch (ValidationException ex)
            {
                response.Errors = ex.Errors.Select(err => err.ErrorMessage).ToList();
                response.StatusCode = "400";
                response.IsSuccess = false;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
                response.IsSuccess = false;
                response.StatusCode = StatusCodes.Status500InternalServerError.ToString();
                return StatusCode(500, response);
            }

        }
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult<Result>> DeleteUserRole(int id)
        {
            Response<object> response = new Response<object>();
            try
            {
                response = await Mediator.Send(new DeleteDepartment(id));
                if (response.IsSuccess)
                    return Ok(response);
                return BadRequest(response);
            }
            catch (ValidationException ex)
            {
                response.Errors = ex.Errors.Select(err => err.ErrorMessage).ToList();
                response.StatusCode = "400";
                response.IsSuccess = false;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
                response.IsSuccess = false;
                response.StatusCode = StatusCodes.Status500InternalServerError.ToString();
                return StatusCode(500, response);
            }
        }
    }
}