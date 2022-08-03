using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Domain.Entities;
using OfficeManager.Infrastructure.Common;
using OfficeManager.Infrastructure.Persistence.Interceptors;
using System.Reflection;

namespace OfficeManager.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        private readonly IMediator _mediator;

        private readonly AuditableEntitySaveChangesInterceptor _interceptor;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IMediator mediator, AuditableEntitySaveChangesInterceptor interceptor)
            : base(options)
        {
            _mediator = mediator;
            _interceptor = interceptor;
        }

        public DbSet<DepartmentMaster> DepartmentMasters { get; set; }
        public DbSet<DesignationMaster> DesignationMasters { get; set; }
        public DbSet<ApplicationUserDepartment> ApplicationUserDepartments { get; set; }
        public DbSet<ProfileMaster> Profiles { get; set; }
        public DbSet<Skill> Skill { get; set; }
        public DbSet<SkillLevel> SkillLevel { get; set; }
        public DbSet<SkillRate> SkillRate { get; set; }
        public DbSet<DepartMent> DepartMent { get; set; }
        public DbSet<Designation> Designation { get; set; }
        public DbSet<Role> Role { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_interceptor);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _mediator.DispatchDomainEvents(this);

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}