using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Skills.Queries.GetAllSkillRates;
using OfficeManager.Domain.Entities;

namespace OfficeManager.API.Controllers
{
    [Authorize]
    public class SkillRateController : ApiControllerBase
    {
        [HttpGet]
        [Route("GetAllSkillRate")]
        public async Task<ActionResult<Response<List<SkillRate>>>> GetAllSkillRates()
        {
            try
            {
                var result = await Mediator.Send(new GetAllSkillRatesQuery());
                if (result.StatusCode == "404")
                    return NotFound(result);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Please check data, it is internal server error.");
            }
        }
    }
}