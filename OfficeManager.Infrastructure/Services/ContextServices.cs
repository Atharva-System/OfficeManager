using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Infrastructure.Persistence;

namespace OfficeManager.Infrastructure.Services
{
    public class ContextServices : IContextServices
    {
        private readonly ApplicationDbContext _context;
        public ContextServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetConnectionString()
        {
            return _context.Database.GetDbConnection().ConnectionString;
        }
    }
}
