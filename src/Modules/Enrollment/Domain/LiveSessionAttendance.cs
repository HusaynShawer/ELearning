using ELearning.SharedKernel;

namespace ELearning.Enrollment.Domain;

public class LiveSessionAttendance : BaseEntity
{
    public Guid EnrollmentId { get; set; }
    public Enrollment Enrollment { get; set; } = default!;

    public Guid LiveSessionId { get; set; }
    public DateTime JoinedAtUtc { get; set; }
    public DateTime? LeftAtUtc { get; set; }
}
