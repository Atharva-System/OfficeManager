using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Feature.Skills.Queries;
using OfficeManager.Application.Wrappers.Abstract;

namespace OfficeManager.API.Controllers
{
    //[Authorize]
    public class SkillRateController : ApiControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<IResponse> GetAllSkillRates()
        {
            return await Mediator.Send(new GetAllSkillRates());
        }
    }
}