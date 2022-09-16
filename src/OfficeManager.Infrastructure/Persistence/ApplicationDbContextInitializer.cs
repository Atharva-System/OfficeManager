using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Infrastructure.Persistence
{
    public class ApplicationDbContextInitializer
    {
        private readonly ILogger<ApplicationDbContext> Logger;
        private readonly ApplicationDbContext Context;

        public ApplicationDbContextInitializer(ILogger<ApplicationDbContext> logger, ApplicationDbContext context)
        {
            Logger = logger;
            Context = context;
        }

        public async Task InitializeAsync()
        {
            try
            {
                if (Context.Database.IsSqlServer())
                {
                    await Context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occured while initializing the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync(new CancellationToken());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occured while seeding the database.");
                throw;
            }
        }

        public async Task TrySeedAsync(CancellationToken cancellationToken)
        {
            if (Context.Roles.All(r => !r.Name.Equals("Admin")))
            {
                await Context.Roles.AddAsync(new RoleMaster() { Name = "Admin", Description = "Manage All Application" });
            }

            //var Designation = new DesignationMaster
            //{
            //    Name = "Admin",
            //};
            //Context.DesignationMasters.Add(Designation);

            if (!Context.Employees.Any(a => a.EmployeeNo == 99999))
            {
                Employee employee = new Employee()
                {
                    EmployeeNo = 99999,
                    DesignationId = 1,
                    DepartmentId = 1,
                    EmployeeName = "Admin",
                    Email = "admin@atharvasystem.com",
                    DateOfBirth = DateTime.Now,
                    DateOfJoining = DateTime.Now
                };
                await Context.Employees.AddAsync(employee);
                await Context.SaveChangesAsync();

                // Default users

                var administrator = new UserMaster { EmployeeID = employee.Id, Email = "admin@atharvasystem.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Atharva@123") };

                if (Context.Users.All(u => u.EmployeeID != administrator.EmployeeID))
                    await Context.Users.AddAsync(administrator);

                await Context.SaveChangesAsync();

                if (Context.UserRoleMapping.All(u => u.UserId != administrator.Id && u.RoleId != Context.Roles.First().Id))
                {
                    await Context.UserRoleMapping.AddAsync(new UserRoleMapping()
                    {
                        UserId = administrator.Id,
                        RoleId = Context.Roles.First().Id
                    });
                    await Context.SaveChangesAsync();
                }
            }
        }
    }
}
