using OfficeManager.Application.ApplicationRoles.Queries;
using OfficeManager.Application.ApplicationUsers.Commands.ForgotPasswordConfirmation;
using OfficeManager.Application.ApplicationUsers.Commands.LoginApplicationUser;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userId);

        Task<bool> IsInRoleAsync(string userId, string role);

        Task<bool> AuthorizeAsync(Guid userId, string policyName);

        Task<(Result result, string userId)> CreateAsync(ApplicationUser user, string roleId, string password);

        Task<List<ApplicationRolesDto>> GetApplicationRoles();

        Task<Result> CreateRoleAsync(string roleName);
        Task<LoggedInUserDto> LoginAsync(string userName, string password);
        Task<bool> ForgotPasswordAsync(string email, CancellationToken cancellationToken);

        Task<bool> ForgotPasswordConfirmationAsync(ForgotPasswordConfirmationCommand request, CancellationToken cancellationToken);

        Task<Result> DeleteRoleAsync(string id);
        Task<bool> RoleExists(string name);
    }
}
