using ANY.DuendeIDS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANY.DuendeIDS.Infrastructure.Persistence.Configurations;

public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.ToTable("Roles");
        builder.Property(r => r.Id).HasColumnOrder(0);
        builder.Property(r => r.Name).HasColumnOrder(1);
        builder.Property(r => r.NormalizedName).HasColumnOrder(2);
        builder.Property(r => r.Permissions).HasColumnOrder(3);
        builder.Property(r => r.ConcurrencyStamp).HasColumnOrder(4);
    }
}