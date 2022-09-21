using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Feature.Users.Commands;
using OfficeManager.Application.Wrappers.Abstract;

namespace OfficeManager.API.Controllers
{
    public class UserController : ApiControllerBase
    {
        private readonly IConfiguration Config;
        public UserController(IConfiguration config)
        {
            Config = config;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IResponse> Login([FromBody] LoginUser command)
        {
            return await Mediator.Send(command);
        }
    }
}
