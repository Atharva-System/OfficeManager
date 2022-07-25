using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.ApplicationRoles.Queries;
using OfficeManager.Application.Common.Exceptions;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Infrastructure.Identity
{
    internal class IdentityService : IIdentityService
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public IdentityService(IApplicationDbContext context,UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<(Result result, string userId)> CreateAsync(ApplicationUser user, string roleId, string password)
        { 
            IdentityResult result = await _userManager.CreateAsync(user, password);
            IdentityRole role = await _roleManager.FindByIdAsync(roleId);
            _userManager.AddToRoleAsync(user, role.Id);

            return (result.ToApplicationResult(),user.Id);
        }

        public async Task<List<ApplicationRolesDto>> GetApplicationRoles()
        {
            return await _roleManager.Roles.Select(r => new ApplicationRolesDto
            {
                Id = r.Id,
                Name = r.Name
            }).ToListAsync();
        }

        public async Task<Result> CreateRoleAsync(string roleName)
        {
            var role = new IdentityRole
            {
                Name = roleName
            };
            await _roleManager.CreateAsync(role);
            return Result.Success("Role created successfully.");
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
