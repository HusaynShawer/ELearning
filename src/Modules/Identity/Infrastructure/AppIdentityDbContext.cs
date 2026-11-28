using ELearning.Identity.Domain;
using ELearning.Identity.Infrastructure.Configurations;
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

        b.ApplyConfiguration(new RefreshTokenConfiguration());
    }
}
