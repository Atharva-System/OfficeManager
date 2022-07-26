using OfficeManager.Infrastructure.Persistence;

namespace OfficeManager.API.Services
{
    public class ContextServices
    {
        private readonly ApplicationDbContext _context;
        public ContextServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetConnectionString()
        {
            return "";
        }
    }
}
