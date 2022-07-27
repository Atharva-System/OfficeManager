using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Infrastructure.Persistence
{
    public class ApplicationDbContextInitializer
    {
        private readonly ILogger<ApplicationDbContext> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _service;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationDbContextInitializer(ILogger<ApplicationDbContext> logger, ApplicationDbContext context, IIdentityService service, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _service = service;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task InitializeAsync()
        {
            try
            {
                if(_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occured while initializing the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while seeding the database.");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            var administratorRole = new IdentityRole("Admin");

            if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
            {
                await _roleManager.CreateAsync(administratorRole);
            }

            var Designation = new DesignationMaster
            {
                Name = "Admin",
            };
            _context.DesignationMasters.Add(Designation);
            _context.SaveChanges();

            // Default users

            var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost.com", FirstName="Admin", LastName="Admin", DesignationId = Designation.Id };

            if (_userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                await _userManager.CreateAsync(administrator, "Atharva@123");
                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }
    }
}
