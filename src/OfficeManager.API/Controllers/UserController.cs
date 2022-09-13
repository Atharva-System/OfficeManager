using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OfficeManager.API.Infrastructure.Filters;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Feature.ApplicationUsers.Commands;
using OfficeManager.Application.Wrappers.Abstract;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
