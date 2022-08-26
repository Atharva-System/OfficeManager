using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OfficeManager.Application.ApplicationUsers.Commands.ForgotPassword;
using OfficeManager.Application.ApplicationUsers.Commands.ForgotPasswordConfirmation;
using OfficeManager.Application.ApplicationUsers.Commands.LoginApplicationUser;
using OfficeManager.Application.ApplicationUsers.Commands.RegisterApplicationUser;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Employees.Queries.GetAllEmployees;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OfficeManager.API.Controllers.Identity
{
    public class UserController : ApiControllerBase
    {
        private readonly IConfiguration _config;
        public UserController(IConfiguration config)
        {
            _config = config;
        }

        //[HttpPost]
        //[Route("Register")]
        //public async Task<ActionResult<Result>> Register(RegisterUserCommand command)
        //{
        //    try
        //    {
        //        var result = await Mediator.Send(command);
        //        if (result.Succeeded)
        //            return Ok(result);
        //        return BadRequest(result);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        return BadRequest(Result.Failure(ex.Errors.Select(x => x.ErrorMessage).ToList(), ex.Message));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(Result.Failure(Enumerable.Empty<string>(), ex.Message));
        //    }
        //}

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<Response<LoggedInUserDto>>> Login([FromBody] LoginUserCommand command)
        {
            try
            {
                Response<LoggedInUserDto> response = await Mediator.Send(command);

                if (response != null && !response._IsSuccess && response._Data == null)
                {
                    return BadRequest(response);
                }
                response._Data = GenerateJWT(response._Data);
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                return BadRequest(Result.Failure(ex.Errors.Select(x => x.ErrorMessage).ToList(), ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(Result.Failure(Enumerable.Empty<string>(), ex.Message));
            }
        }

        private LoggedInUserDto GenerateJWT(LoggedInUserDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            IEnumerable<Claim> claims = new Claim[]
            {
                new Claim("Id",user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("EmployeeNo", user.EmployeeNo.ToString()),
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

            user.Token = new JwtSecurityTokenHandler().WriteToken(token);
            return user;
        }

        //[Authorize]
        //[HttpGet]
        //[Route("{id}")]
        //public async Task<ActionResult<UserProfileDto>> GetUserProfile(Guid id)
        //{
        //    return await Mediator.Send(new GetUserProfileQuery(id));
        //}

        //[HttpPost]
        //[Route("ForgotPassword")]
        //public async Task<ActionResult<bool>> ForgotPassword(ForgotPasswordCommand command)
        //{
        //    try
        //    {
        //        return await Mediator.Send(command);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        return BadRequest(Result.Failure(ex.Errors.Select(x => x.ErrorMessage).ToList(), ex.Message));
        //    }
        //    catch(Exception ex)
        //    {
        //        return BadRequest(Result.Failure(Enumerable.Empty<string>(), ex.Message));
        //    }
        //}

        //[HttpPost]
        //[Route("ForgotPassword/Confirmation")]
        //public async Task<ActionResult<bool>> ForgotPasswordConfirmation(ForgotPasswordConfirmationCommand command)
        //{
        //    return await Mediator.Send(command);
        //}
    }
}
