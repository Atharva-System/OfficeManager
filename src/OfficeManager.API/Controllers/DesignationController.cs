using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Designations.Queries.SearchDesignations;

namespace OfficeManager.API.Controllers
{
    [Authorize]
    public class DesignationController : ApiControllerBase
    {
        [HttpGet]
        [Route("GetAllDesignation")]
        public async Task<ActionResult<Response<List<DesignationDTO>>>> SearchDesignation(string? search)
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
    }
}