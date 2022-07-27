using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.ApplicationRoles.Queries;
using OfficeManager.Application.ApplicationUsers.Commands.ForgotPasswordConfirmation;
using OfficeManager.Application.ApplicationUsers.Commands.LoginApplicationUser;
using OfficeManager.Application.Common.Exceptions;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Employees.Queries.GetAllEmployees;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Infrastructure.Identity
{
    internal class IdentityService : IIdentityService
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        public IdentityService(IApplicationDbContext context,UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }

        public async Task<(Result result, string userId)> CreateAsync(ApplicationUser user, string roleId, string password)
        { 
            IdentityResult result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                IdentityRole role = await _roleManager.FindByIdAsync(roleId);
                result = await _userManager.AddToRoleAsync(user, role.Name);

                return (result.ToApplicationResult(), user.Id);
            }
            else
            {
                return (result.ToApplicationResult(), "");
            }
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

        public async Task<LoggedInUserDto> LoginAsync(string userName,string password)
        {
            LoggedInUserDto loggedInUser = new LoggedInUserDto();
            var user = await _userManager.FindByNameAsync(userName);
            if(user != null && (await _userManager.CheckPasswordAsync(user,password)))
            {
                loggedInUser.Email = user.Email;
                loggedInUser.UserId = user.Id;
                loggedInUser.UserName = userName;
                loggedInUser.Roles = (await _userManager.GetRolesAsync(user)).ToList();
                loggedInUser.Token = "";
                return loggedInUser;
            }
            return null;
        }

        public async Task<bool> AuthorizeAsync(Guid userId, string policyName)
        {
            return true;
        }

        public async Task<bool> ForgotPasswordAsync(string email,CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new NotFoundException();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callback = "http://localhost:4200/forgotpassword/confirmation";
            var message = new OfficeManager.Application.Common.Models.Message(new string[] { user.Email }, "Reset password token", callback, null);
            await _emailSender.SendEmailAsync(message,cancellationToken);
            return true;
        }

        public async Task<Result> DeleteRoleAsync(string id)
        {
            IdentityRole userRole = await _roleManager.FindByIdAsync(id);
            await _roleManager.DeleteAsync(userRole);
            return Result.Success("Role deleted successfully");
        }

        public async Task<bool> ForgotPasswordConfirmationAsync(ForgotPasswordConfirmationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new NotFoundException();
            var resetPasswordResult = await _userManager.ResetPasswordAsync(user,request.Token,request.Password);
            if(resetPasswordResult.Succeeded)
                return true;
            return false;
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("User is not registered");

            return user.UserName;
        }

        public async Task<bool> RoleExists(string name)
        {
            var role = await _roleManager.FindByNameAsync(name);
            if (role == null)
                return false;
            return true;
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("User is not registered");


            var inRole = await _userManager.IsInRoleAsync(user, role);

            if (!inRole)
                throw new NotFoundException("Role is not specified in database.");

            return true;
        }

        //public async Task<List<EmployeeDto>> GetAllEmployeesAsync(GetAllEmployeesQuery request)
        //{
        //    List<ApplicationUser> users = await _userManager.Users.ToListAsync();
        //    if(!string.IsNullOrEmpty(request.Search))
        //    {
        //        users = users.Where(u => u.FirstName.Contains(request.Search) || u.LastName.Contains(request.Search) || u.Email.Contains(request.Search) || u.UserName.Contains(request.Search)).ToList();
        //    }
        //    if(request.DesignationId != Guid.Empty && request.DesignationId != null)
        //    {
        //        users = users.Where(u => u.DesignationId == request.DesignationId).ToList();
        //    }
        //    if(request.DepartmentId != Guid.Empty && request.DepartmentId != null)
        //    {
        //        users = users.Where(u => _context.ApplicationUserDepartments.Where(aud => aud.DepartmentId == request.DepartmentId && aud.UserId == u.Id).Count() > 0).ToList();
        //    }
        //    if(!string.IsNullOrEmpty(request.RoleId))
        //    {
        //        users = users.Where(u => )
        //    }
        //}
    }
}
