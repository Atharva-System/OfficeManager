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
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<Response<List<DesignationDTO>>>> SearchDesignation(string? search)
        {
            try
            {
                var result = await Mediator.Send(new SearchDesignations { Search = search });
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