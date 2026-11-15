using ELearning.SharedKernel;

namespace ELearning.Enrollment.Domain;

public class LessonProgress : BaseEntity
{
    public Guid EnrollmentId { get; set; }
    public Enrollment Enrollment { get; set; } = default!;

    public Guid LessonId { get; set; }
    public bool IsCompleted { get; set; }
    public int LastPositionSeconds { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
}
