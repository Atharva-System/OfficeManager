using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Domain.Entities;
using OfficeManager.Infrastructure.Common;
using OfficeManager.Infrastructure.Persistence.Interceptors;
using System.Reflection;

namespace OfficeManager.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext,IApplicationDbContext
    {
        private readonly IMediator _mediator;
        private readonly AuditableEntitySaveChangesInterceptor _interceptor;
        private IDbContextTransaction _transaction;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IMediator mediator, AuditableEntitySaveChangesInterceptor interceptor)
            :base(options)
        {
            _mediator = mediator;
            _interceptor = interceptor;
        }
        public DbSet<RoleMaster> Roles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<UserMaster> Users { get; set; }
        public DbSet<UserRoleMapping> UserRoleMapping { get; set; }
        public string GetConnectionString { get => this.Database.GetDbConnection().ConnectionString; }

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
            try
            {
                await _mediator.DispatchDomainEvents(this);

                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _transaction.Rollback();
                throw ex;
            }
        }

        public async void BeginTransaction()
        {
            try
            {
                _transaction = this.Database.BeginTransaction();
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
                _transaction?.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
