using Microsoft.EntityFrameworkCore;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public string GetConnectionString { get;}
        DbSet<UserMaster> Users { get; set; }
        DbSet<RoleMaster> Roles { get; set; }
        DbSet<Employee> Employees { get; set; }
        DbSet<UserRoleMapping> UserRoleMapping { get; set; }
        DbSet<Client> Client { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        void BeginTransaction();
        void CommitTransaction();
    }
}
