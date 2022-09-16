using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : IRequest
    {
        private readonly ILogger Logger;
        
        private readonly ICurrentUserServices CurrentUserService;

        public LoggingBehaviour(ILogger logger, ICurrentUserServices currentUserServices)
        {
            Logger = logger;
            CurrentUserService = currentUserServices;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            Logger.LogInformation("Office Manager Request: {Name} {@UserId} {@UserName} {@Request}",
                typeof(TRequest).Name, CurrentUserService.loggedInUser.UserId, CurrentUserService.loggedInUser.EmployeeNo, request);
        }
    }
}
