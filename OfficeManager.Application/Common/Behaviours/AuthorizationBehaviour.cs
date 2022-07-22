using MediatR;
using OfficeManager.Application.Common.Exceptions;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Security;
using System.Reflection;

namespace OfficeManager.Application.Common.Behaviours
{
    public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ICurrentUserService currentUserService;
        private readonly IApplicationDbContext _context;

        public AuthorizationBehaviour(ICurrentUserService currentUserService,IApplicationDbContext context)
        {
            this.currentUserService = currentUserService;
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var authorizationAttribute = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

            if(authorizationAttribute.Any())
            {
                if (currentUserService.UserId == null)
                {
                    throw new UnauthorizedAccessException();
                }

                var authorizeAttributesWithRoles = authorizationAttribute.Where(a => !string.IsNullOrWhiteSpace(a.Roles));

                if (authorizeAttributesWithRoles.Any())
                {
                    var authorized = false;

                    foreach (var roles in authorizeAttributesWithRoles.Select(a => a.Roles.Split(',')))
                    {
                        foreach (var role in roles)
                        {
                            var userRole = _context.UserRole.FirstOrDefault(r => r.Title == role);
                            var user = _context.UserMaster.Where(u => u.Id == currentUserService.UserId && u.RoleId == userRole.Id);
                            if (user.Any())
                            {
                                authorized = true;
                                break;
                            }
                        }
                    }

                    // Must be a member of at least one role in roles
                    if (!authorized)
                    {
                        throw new ForbiddenAccessException();
                    }
                }
            }
            return await next();
        }
    }
}
