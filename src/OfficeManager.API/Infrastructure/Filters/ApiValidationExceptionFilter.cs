using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OfficeManager.Application.Wrappers.Concrete;

namespace OfficeManager.API.Infrastructure.Filters
{
    public class ApiValidationExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if(context.Exception is ValidationException)
            {
                var exception = (ValidationException)context.Exception;
                context.Result = new BadRequestObjectResult( new ErrorResponse(StatusCodes.Status400BadRequest.ToString(), exception.Errors.Select(err => err.ErrorMessage).ToList()));
                return;
            }

        }
    }
}
