using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.ApplicationRoles.Queries;
using OfficeManager.Application.Feature.UserRoles.Commands;
using OfficeManager.Application.Feature.UserRoles.Queries;
using OfficeManager.Application.Wrappers.Abstract;

namespace OfficeManager.API.Controllers
{
    //[Authorize]
    [Route("api/v1/UserRoles")]
    public class UserRoleController : ApiControllerBase
    {
        [HttpPost]
        [Route("Add")]
        public async Task<IResponse> CreateApplicationRole(CreateUserRoles command)
        {
            return await Mediator.Send(command);
        }
        //return all the deaprtments usefull for dropdown like usage
        //[Authorize]
        [HttpGet]
        [Route("All")]
        public async Task<IResponse> GetAll()
        {
            return await Mediator.Send(new GetUserRoles());
        }

        //return paginated result useful for search and listing features
        [HttpGet]
        [Route("")]
        public async Task<IResponse> Search([FromQuery] SearchUserRoles query)
        {
            return await Mediator.Send(query);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IResponse> DeleteUserRole(int id)
        {
            return await Mediator.Send(new DeleteUserRole(id));
        }
    }
}
