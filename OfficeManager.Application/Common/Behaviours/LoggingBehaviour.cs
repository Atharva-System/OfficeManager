using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : IRequest
    {
        private readonly ILogger _logger;
        
        private readonly ICurrentUserService _currentUserService;
        
        private readonly IIdentityService _identityService;

        public LoggingBehaviour(ILogger logger, ICurrentUserService currentUserService, IIdentityService identityService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.UserId;
            string userName = string.Empty;

            if(userId != null)
            {
                userName = await _identityService.GetUserNameAsync(userId);
            }

            _logger.LogInformation("Office Manager Request: {Name} {@UserId} {@UserName} {@Request}",
                requestName, userId, userName, request);
        }
    }
}
