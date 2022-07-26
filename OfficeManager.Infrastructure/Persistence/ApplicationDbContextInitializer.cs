using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Infrastructure.Persistence
{
    public class ApplicationDbContextInitializer
    {
        private readonly ILogger<ApplicationDbContext> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _service;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApplicationDbContextInitializer(ILogger<ApplicationDbContext> logger, ApplicationDbContext context, IIdentityService service, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _service = service;
            _roleManager = roleManager;
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
            var administratorRole = new IdentityRole
            {
                Name = "Administrator"
            };

            if(_roleManager.Roles.All(r => r.Name != "Admin"))
            {
                await _roleManager.CreateAsync(administratorRole);
            }
        }
    }
}
