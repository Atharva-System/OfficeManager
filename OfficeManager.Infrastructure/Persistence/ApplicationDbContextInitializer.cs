using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Infrastructure.Persistence
{
    public class ApplicationDbContextInitializer
    {
        private readonly ILogger<ApplicationDbContext> _logger;
        private readonly ApplicationDbContext _context;

        public ApplicationDbContextInitializer(ILogger<ApplicationDbContext> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
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
            var administratorRole = new UserRole
            {
                Title = "Administrator",
                Description = "Who own the whole system. He can access with this role. And he/she can do everything provided by the system."
            };

            if(_context.UserRole.All(r => r.Title != "Administrator"))
            {
                _context.UserRole.Add(administratorRole);
                _context.SaveChangesAsync();
            }
        }
    }
}
