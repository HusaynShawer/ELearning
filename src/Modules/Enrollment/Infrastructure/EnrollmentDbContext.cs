using ELearning.Enrollment.Domain;
using Microsoft.EntityFrameworkCore;

namespace ELearning.Enrollment.Infrastructure;

public class EnrollmentDbContext : DbContext
{
    public const string Schema = "enrollment";

    public EnrollmentDbContext(DbContextOptions<EnrollmentDbContext> options) : base(options) { }

    public DbSet<Domain.Enrollment> Enrollments => Set<Domain.Enrollment>();
    public DbSet<LessonProgress> LessonProgresses => Set<LessonProgress>();
    public DbSet<LiveSessionAttendance> Attendances => Set<LiveSessionAttendance>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.HasDefaultSchema(Schema);

        b.Entity<Domain.Enrollment>(e =>
        {
            e.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
            e.HasIndex(x => new { x.StudentId, x.CourseId }).IsUnique();
            e.HasIndex(x => x.CourseId);
        });

        b.Entity<LessonProgress>(e =>
        {
            e.HasIndex(x => new { x.EnrollmentId, x.LessonId }).IsUnique();
            e.HasOne(x => x.Enrollment).WithMany(x => x.LessonProgresses)
             .HasForeignKey(x => x.EnrollmentId).OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<LiveSessionAttendance>(e =>
        {
            e.HasIndex(x => new { x.EnrollmentId, x.LiveSessionId }).IsUnique();
            e.HasOne(x => x.Enrollment).WithMany(x => x.Attendances)
             .HasForeignKey(x => x.EnrollmentId).OnDelete(DeleteBehavior.Cascade);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        foreach (var e in ChangeTracker.Entries<ELearning.SharedKernel.BaseEntity>()
                     .Where(x => x.State == EntityState.Modified))
            e.Entity.UpdatedAtUtc = DateTime.UtcNow;
        return base.SaveChangesAsync(ct);
    }
}
