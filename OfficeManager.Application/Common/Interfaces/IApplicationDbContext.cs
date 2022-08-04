using Microsoft.EntityFrameworkCore;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<DepartmentMaster> DepartmentMasters { get; set; }
        DbSet<DesignationMaster> DesignationMasters { get; set; }
        DbSet<ApplicationUserDepartment> ApplicationUserDepartments { get; set; }
        DbSet<ProfileMaster> Profiles { get; set; }

        DbSet<Client> Client { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
