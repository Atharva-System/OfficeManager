using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Infrastructure.Persistence
{
    public class ApplicationDbContextInitializer
    {
        private readonly ILogger<ApplicationDbContext> logger;
        private readonly ApplicationDbContext context;

        public ApplicationDbContextInitializer(ILogger<ApplicationDbContext> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public async Task InitializeAsync()
        {
            try
            {
                if (context.Database.IsSqlServer())
                {
                    await context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while initializing the database.");
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
                logger.LogError(ex, "An error occured while seeding the database.");
                throw;
            }
        }

        public async Task TrySeedAsync(CancellationToken cancellationToken)
        {
            if (context.Roles.All(r => !r.Name.Equals("Admin")))
            {
                context.Roles.Add(new RoleMaster() { Name = "Admin", Description = "Manage All Application" });
            }

            //var Designation = new DesignationMaster
            //{
            //    Name = "Admin",
            //};
            //context.DesignationMasters.Add(Designation);

            if(!context.Employees.Any(a => a.EmployeeNo == 99999))
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
                context.Employees.Add(employee);
                context.SaveChanges();

                // Default users

                var administrator = new UserMaster { EmployeeID = employee.Id, Email = "admin@atharvasystem.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Atharva@123") };

                if (context.Users.All(u => u.EmployeeID != administrator.EmployeeID))
                    context.Users.Add(administrator);

                context.SaveChanges();

                if (context.UserRoleMapping.All(u => u.UserId != administrator.Id && u.RoleId != context.Roles.First().Id))
                {
                    context.UserRoleMapping.Add(new UserRoleMapping()
                    {
                        UserId = administrator.Id,
                        RoleId = context.Roles.First().Id
                    });
                    context.SaveChanges();
                }

                
            }
        }
    }
}
