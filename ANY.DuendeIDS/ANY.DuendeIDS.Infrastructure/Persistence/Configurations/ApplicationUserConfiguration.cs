using ANY.DuendeIDS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANY.DuendeIDS.Infrastructure.Persistence.Configurations;

public class ApplicationUserConfiguraiton : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("Users");

        builder.Property(u => u.Name).IsRequired();

        builder.Property(u => u.ProfilePictureUrl)
            .IsRequired()
            .HasDefaultValue("../../../assets/images/avatars/user.png");
    }
}