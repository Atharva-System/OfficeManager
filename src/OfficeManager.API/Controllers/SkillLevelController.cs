using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Feature.Skills.Queries;
using OfficeManager.Application.Wrappers.Abstract;

namespace OfficeManager.API.Controllers
{
    //[Authorize]
    public class SkillLevelController : ApiControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<IResponse> GetAllSkillLevel()
        {
            return await Mediator.Send(new GetAllSkillLevels());
        }
    }
}