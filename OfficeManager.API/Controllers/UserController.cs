using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.UserMasters.Commands.AuthenticateUser;
using OfficeManager.Application.UserMasters.Commands.RegisterUser;
using OfficeManager.Application.UserMasters.Queries.GetUserProfile;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OfficeManager.API.Controllers
{
    public class UserController : ApiControllerBase
    {
        private readonly IConfiguration _config;
        public UserController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<Result>> Register(RegisterUserCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<LoggedInUserDto>> Login(AuthenticateUserCommand command)
        {
            var user = await Mediator.Send(command);
            if(user == null)
            {
                return BadRequest("Authentication failed please check username and password.");
            }
            return GenerateJWT(user);
        }

        private LoggedInUserDto GenerateJWT(LoggedInUserDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            IEnumerable<Claim> claims = new Claim[]
            {
                new Claim("Id",user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddDays(30).ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Aud, _config["JWT:ValidAudience"]),
                new Claim(JwtRegisteredClaimNames.Iss, _config["JWT:ValidIssuer"])
            };
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                null,
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credential
                );
            user.Token =  new JwtSecurityTokenHandler().WriteToken(token);
            return user;
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile(Guid id)
        {
            return await Mediator.Send(new GetUserProfileQuery(id));
        }
    }
}
