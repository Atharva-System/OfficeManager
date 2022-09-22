using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Feature.Skills.Commands;
using OfficeManager.Application.Feature.Skills.Queries;
using OfficeManager.Application.Wrappers.Abstract;

namespace OfficeManager.API.Controllers
{
    //[Authorize]
    public class SkillController : ApiControllerBase
    {
        //return paginated result useful for search and listing features
        [HttpGet]
        [Route("")]
        public async Task<IResponse> Search([FromQuery] SearchSkills query)
        {
            return await Mediator.Send(query);
        }
        //return all the deaprtments usefull for dropdown like usage
        [HttpGet]
        [Route("All")]
        public async Task<IResponse> GetAll()
        {
            return await Mediator.Send(new GetAllSkills());
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IResponse> AddSkill(CreateSkill command)
        {
            return await Mediator.Send(command);
        }
    }
}