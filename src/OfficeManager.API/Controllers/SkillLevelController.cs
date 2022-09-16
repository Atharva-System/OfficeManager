using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Feature.Skills.Queries;
using OfficeManager.Domain.Entities;

namespace OfficeManager.API.Controllers
{
    [Authorize]
    public class SkillLevelController : ApiControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<Response<List<SkillLevel>>>> GetAllSkillLevel()
        {
            try
            {
                var result = await Mediator.Send(new GetAllSkillLevels());
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