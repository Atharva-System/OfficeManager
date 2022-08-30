using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : IRequest
    {
        private readonly ILogger logger;
        
        private readonly ICurrentUserServices currentUserService;

        public LoggingBehaviour(ILogger logger, ICurrentUserServices currentUserServices)
        {
            this.logger = logger;
            this.currentUserService = currentUserServices;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Office Manager Request: {Name} {@UserId} {@UserName} {@Request}",
                typeof(TRequest).Name, currentUserService.loggedInUser.UserId, currentUserService.loggedInUser.EmployeeNo, request);
        }
    }
}
