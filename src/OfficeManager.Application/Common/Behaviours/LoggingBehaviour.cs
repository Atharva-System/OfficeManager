using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : IRequest
    {
        private readonly ILogger _logger;
        
        private readonly ICurrentUserServices _currentUserService;

        public LoggingBehaviour(ILogger logger, ICurrentUserServices currentUserServices)
        {
            _logger = logger;
            _currentUserService = currentUserServices;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Office Manager Request: {Name} {@UserId} {@UserName} {@Request}",
                typeof(TRequest).Name, _currentUserService.loggedInUser.UserId, _currentUserService.loggedInUser.EmployeeNo, request);
        }
    }
}
