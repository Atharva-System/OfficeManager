using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Feature.Designations.Queries;
using OfficeManager.Application.Wrappers.Abstract;

namespace OfficeManager.API.Controllers
{
    [Authorize]
    public class DesignationController : ApiControllerBase
    {
        //return all the deaprtments usefull for dropdown like usage
        [HttpGet]
        [Route("All")]
        public async Task<IResponse> GetAll()
        {
            return await Mediator.Send(new GetAllDesignations());
        }
        //return paginated result useful for search and listing features
        [HttpGet]
        [Route("")]
        public async Task<IResponse> Search([FromQuery] SearchDesignations query)
        {
            return await Mediator.Send(query);
        }
    }
}