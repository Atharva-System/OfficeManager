using Microsoft.EntityFrameworkCore;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<UserMaster> UserMaster { get; set; }
        DbSet<UserProfile> UserProfile { get; set; }
        DbSet<UserRole> UserRole { get; set; }
        DbSet<DepartmentMaster> DepartmentMasters { get; set; }
        DbSet<DesignationMaster> DesignationMasters { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
