using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Domain.Entities;
using OfficeManager.Infrastructure.Common;
using OfficeManager.Infrastructure.Persistence.Interceptors;
using System.Reflection;

namespace OfficeManager.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext,IApplicationDbContext
    {
        private readonly IMediator Mediator;
        private readonly AuditableEntitySaveChangesInterceptor Interceptor;
        private IDbContextTransaction Transaction;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IMediator mediator, AuditableEntitySaveChangesInterceptor interceptor)
            : base(options)
        {
            Mediator = mediator;
            Interceptor = interceptor;
        }
        public DbSet<RoleMaster> Roles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<UserMaster> Users { get; set; }
        public DbSet<UserRoleMapping> UserRoleMapping { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<ProjectMaster> Projects { get; set; }
        public string GetConnectionString { get => this.Database.GetDbConnection().ConnectionString; }

        public DbSet<Skill> Skill { get; set; }
        public DbSet<SkillLevel> SkillLevel { get; set; }
        public DbSet<SkillRate> SkillRate { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Designation> Designation { get; set; }
        public DbSet<EmployeeSkill> EmployeeSkills { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(Interceptor);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await Mediator.DispatchDomainEvents(this);

                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Transaction.Rollback();
                throw ex;
            }
        }

        public async Task<int> SaveChangesWithoutLogAsync(CancellationToken cancellationToken =default)
        {
            try
            {
                await Mediator.DispatchDomainEvents(this);

                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Transaction.Rollback();
                throw ex;
            }
        }

        public async void BeginTransaction()
        {
            try
            {
                Transaction = this.Database.BeginTransaction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async void CommitTransaction()
        {
            try
            {
                Transaction?.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}