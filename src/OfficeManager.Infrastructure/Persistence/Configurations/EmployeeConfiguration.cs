using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Infrastructure.Persistence.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(p => p.DepartmentId)
                .IsRequired(false);

            builder.Property(p => p.DesignationId)
                .IsRequired(false);

            builder.HasOne<Department>(d => d.Department).WithMany(e => e.Employees).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<Designation>(d => d.Designation).WithMany(e => e.Employees).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
