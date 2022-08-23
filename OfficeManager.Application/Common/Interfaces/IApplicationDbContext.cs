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
        DbSet<ProjectMaster> Projects { get; set; }
        DbSet<DepartMent> DepartMent { get; set; }
        DbSet<Designation> Designation { get; set; }
        DbSet<Skill> Skill { get; set; }
        DbSet<SkillLevel> SkillLevel { get; set; }
        DbSet<SkillRate> SkillRate { get; set; }
        DbSet<EmployeeSkill> EmployeeSkills { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesWithoutLogAsync(CancellationToken cancellationToken);
        void BeginTransaction();
        void CommitTransaction();
    }
}
