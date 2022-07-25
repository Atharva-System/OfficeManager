using OfficeManager.Application.ApplicationRoles.Queries;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(Guid userId);

        Task<bool> IsInRoleAsync(Guid userId, string role);

        Task<bool> AuthorizeAsync(Guid userId, string policyName);

        Task<(Result result, string userId)> CreateAsync(ApplicationUser user, string roleId, string password);

        Task<List<ApplicationRolesDto>> GetApplicationRoles();

        Task<Result> CreateRoleAsync(string roleName);
    }
}
