using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OfficeManager.Application.Common.Exceptions;
using OfficeManager.Application.Wrappers.Concrete;

namespace OfficeManager.API.Infrastructure.Filters
{
    public class AccessExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if(context.Exception is UnauthorizedAccessException)
            {
                context.Result = new UnauthorizedObjectResult(new ErrorResponse(401, context.Exception.Message));
                return;
            }
            if (context.Exception is ForbiddenAccessException)
            {
                context.Result = new UnauthorizedObjectResult(new ErrorResponse(401, context.Exception.Message));
                return;
            }
        }
    }
}
