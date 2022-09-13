using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Common.Exceptions;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Feature.ApplicationUsers.Commands;

namespace OfficeManager.API.Controllers
{
    public class UserController : ApiControllerBase
    {
        private readonly IConfiguration Config;
        public UserController(IConfiguration config)
        {
            Config = config;
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
        public async Task<ActionResult<Response<TokenDTO>>> Login([FromBody] LoginUser command)
        {
            try
            {
                Response<TokenDTO> response = await Mediator.Send(command);

                if (response != null && !response.IsSuccess && response.Data == null)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(Result.Failure(ex.Errors.Select(x => x.ErrorMessage).ToList(), ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(Result.Failure(Enumerable.Empty<string>(), ex.Message));
            }
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<ActionResult<Response<TokenDTO>>> RefreshToken(CreateTokenByRefreshToken command)
        {
            try
            {
                Response<TokenDTO> response = await Mediator.Send(command);

                if (response != null && !response.IsSuccess && response.Data == null)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(Result.Failure(Enumerable.Empty<string>(), ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(Result.Failure(Enumerable.Empty<string>(), ex.Message));
            }
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
