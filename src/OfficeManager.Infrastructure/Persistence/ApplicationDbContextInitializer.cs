using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Infrastructure.Persistence
{
    public class ApplicationDbContextInitializer
    {
        private readonly ILogger<ApplicationDbContext> _logger;
        private readonly ApplicationDbContext _context;

        public ApplicationDbContextInitializer(ILogger<ApplicationDbContext> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task InitializeAsync()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while initializing the database.");
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
                _logger.LogError(ex, "An error occured while seeding the database.");
                throw;
            }
        }

        public async Task TrySeedAsync(CancellationToken cancellationToken)
        {
            if (_context.Roles.All(r => !r.Name.Equals("Admin")))
            {
                _context.Roles.Add(new RoleMaster() { Name = "Admin", Description = "Manage All Application" });
            }

            //var Designation = new DesignationMaster
            //{
            //    Name = "Admin",
            //};
            //_context.DesignationMasters.Add(Designation);

            if(!_context.Employees.Any(a => a.EmployeeNo == 99999))
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
                _context.Employees.Add(employee);
                _context.SaveChanges();

                // Default users

                var administrator = new UserMaster { EmployeeID = employee.Id, Email = "admin@atharvasystem.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Atharva@123") };

                if (_context.Users.All(u => u.EmployeeID != administrator.EmployeeID))
                    _context.Users.Add(administrator);

                _context.SaveChanges();

                if (_context.UserRoleMapping.All(u => u.UserId != administrator.Id && u.RoleId != _context.Roles.First().Id))
                {
                    _context.UserRoleMapping.Add(new UserRoleMapping()
                    {
                        UserId = administrator.Id,
                        RoleId = _context.Roles.First().Id
                    });
                    _context.SaveChanges();
                }

                
            }
        }
    }
}
