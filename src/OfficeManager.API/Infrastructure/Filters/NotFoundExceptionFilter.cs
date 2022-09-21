using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OfficeManager.Application.Common.Exceptions;
using OfficeManager.Application.Wrappers.Concrete;

namespace OfficeManager.API.Infrastructure.Filters
{
    public class NotFoundExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if(context.Exception is NotFoundException)
            {
                context.Result = new NotFoundObjectResult(new ErrorResponse(StatusCodes.Status404NotFound.ToString(), context.Exception.Message));
                return;
            }
        }
    }
}
