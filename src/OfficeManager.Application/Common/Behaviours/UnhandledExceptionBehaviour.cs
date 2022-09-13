using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace OfficeManager.Application.Common.Behaviours
{
    internal class UnhandledExceptionBehaviour<TRequest,TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> Logger;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
        {
            Logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch(ValidationException vex)
            {
                var requestName = typeof(TRequest).Name;

                Logger.LogError(vex, "Office Manager Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

                throw;
            }
            catch(Exception ex)
            {
                var requestName = typeof(TRequest).Name;

                Logger.LogError(ex, "Office Manager Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

                throw;
            }
        }
    }
}
