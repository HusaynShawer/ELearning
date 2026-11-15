using ELearning.SharedKernel;

namespace ELearning.Enrollment.Domain;

public enum EnrollmentStatus { Active = 1, Completed = 2, Cancelled = 3, Expired = 4 }

public class Enrollment : BaseEntity
{
    // Ids only: StudentId -> Identity module, CourseId -> Catalog module
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }

    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;
    public DateTime EnrolledAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }

    public ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();
    public ICollection<LiveSessionAttendance> Attendances { get; set; } = new List<LiveSessionAttendance>();
}
