﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Domain.Common;
using OfficeManager.Domain.Entities;
using OfficeManager.Infrastructure.Common;
using OfficeManager.Infrastructure.Persistence.Interceptors;
using OfficeManager.Infrastructure.Services;
using System.Reflection;

namespace OfficeManager.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext,IApplicationDbContext
    {
        private readonly IMediator Mediator;
        private readonly AuditableEntitySaveChangesInterceptor Interceptor;
        private IDbContextTransaction Transaction;
        private readonly ICurrentUserServices CurrentUserService;

        public ApplicationDbContext(
            ICurrentUserServices currentUserService,
            DbContextOptions<ApplicationDbContext> options,
            IMediator mediator, AuditableEntitySaveChangesInterceptor interceptor)
            : base(options)
        {
            Mediator = mediator;
            Interceptor = interceptor;
            CurrentUserService = currentUserService;
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
        public DbSet<RefreshToken> RefreshToken { get; set; }

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

                foreach (var entry in this.ChangeTracker.Entries<BaseAuditableEntity>())
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = CurrentUserService.loggedInUser != null ? CurrentUserService.loggedInUser.UserId : 0;
                    }

                    if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
                    {
                        entry.Entity.ModifiedBy = CurrentUserService.loggedInUser != null ? CurrentUserService.loggedInUser.UserId : 0;
                        entry.Entity.ModifiedDate = DateTime.Now;
                    }
                }

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

                foreach (var entry in this.ChangeTracker.Entries<BaseAuditableEntity>())
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = CurrentUserService.loggedInUser != null ? CurrentUserService.loggedInUser.UserId : 0;
                    }

                    if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
                    {
                        entry.Entity.ModifiedBy = CurrentUserService.loggedInUser != null ? CurrentUserService.loggedInUser.UserId : 0;
                        entry.Entity.ModifiedDate = DateTime.Now;
                    }
                }

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