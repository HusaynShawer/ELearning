using ELearning.Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ELearning.Identity.Infrastructure;

public class AppIdentityDbContext
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public const string Schema = "identity";

    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options) { }

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);          // must be first
        b.HasDefaultSchema(Schema);

        // Cleaner table names (AspNetUsers -> Users, ...)
        b.Entity<ApplicationUser>(e =>
        {
            e.ToTable("Users");
            e.Property(x => x.FullName).HasMaxLength(150).IsRequired();
        });
        b.Entity<ApplicationRole>().ToTable("Roles");
        b.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
        b.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
        b.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
        b.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");
        b.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");

        b.Entity<RefreshToken>(e =>
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
        });
    }
}
