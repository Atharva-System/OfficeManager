using OfficeManager.Application.Common.Exceptions;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Infrastructure.Identity
{
    internal class IdentityService : IIdentityService
    {
        private readonly IApplicationDbContext _context;
        public IdentityService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AuthorizeAsync(Guid userId, string policyName)
        {
            return true;
        }

        public async Task<string> GetUserNameAsync(Guid userId)
        {
            var user = _context.UserMaster.FirstOrDefault(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException("User is not registered");

            return user.Email;
        }

        public async Task<bool> IsInRoleAsync(Guid userId, string role)
        {
            var user = _context.UserMaster.FirstOrDefault(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException("User is not registered");

            var userRole = _context.UserRole.Where(r => r.Id == user.RoleId && r.Title == role);

            if(!userRole.Any())
                throw new NotFoundException("Role is not specified in database.");

            return true;
        }
    }
}
