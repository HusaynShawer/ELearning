using ELearning.Identity.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Identity.Infrastructure.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> e)
    {
        e.ToTable("RefreshTokens");
        e.HasKey(x => x.Id);
        e.Property(x => x.TokenHash).HasMaxLength(64).IsRequired();
        e.Property(x => x.ReplacedByTokenHash).HasMaxLength(64);
        e.HasIndex(x => x.TokenHash).IsUnique();
        e.Ignore(x => x.IsActive);
        e.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
