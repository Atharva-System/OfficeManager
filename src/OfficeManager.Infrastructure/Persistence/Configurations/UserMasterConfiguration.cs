using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Infrastructure.Persistence.Configurations
{
    public class UserMasterConfiguration : IEntityTypeConfiguration<UserMaster>
    {
        public void Configure(EntityTypeBuilder<UserMaster> builder)
        {
            builder.Property(p => p.PasswordHash)
                .IsRequired();
        }
    }
}
