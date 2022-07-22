using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Infrastructure.Persistence.Configurations
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.Property(p => p.Contact)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(p => p.PersonalEmail)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(p => p.DateOfJoining)
                .IsRequired();
        }
    }
}
