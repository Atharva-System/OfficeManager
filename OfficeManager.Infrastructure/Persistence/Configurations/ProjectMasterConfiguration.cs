using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Infrastructure.Persistence.Configurations
{
    public class ProjectMasterConfiguration : IEntityTypeConfiguration<ProjectMaster>
    {
        public void Configure(EntityTypeBuilder<ProjectMaster> builder)
        {
            builder.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(400);
        }
    }
}
