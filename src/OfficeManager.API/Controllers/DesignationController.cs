using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Feature.Designations.Queries;

namespace OfficeManager.API.Controllers
{
    [Authorize]
    public class DesignationController : ApiControllerBase
    {
        //return all the deaprtments usefull for dropdown like usage
        [HttpGet]
        [Route("All")]
        public async Task<ActionResult<Response<List<DesignationDTO>>>> GetAll()
        {
            try
            {
                var result = await Mediator.Send(new GetAllDesignations());
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
        public async Task<ActionResult<Response<PaginatedList<DesignationDTO>>>> Search([FromQuery] SearchDesignations query)
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
    }
}