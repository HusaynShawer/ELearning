using ELearning.Enrollment.Domain;
namespace ELearning.Enrollment.Application;
public class EnrollmentDto
{
    // public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }

    // public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;
    // public DateTime EnrolledAtUtc { get; set; } = DateTime.UtcNow;
    // public DateTime? ExpiresAtUtc { get; set; }
    // public DateTime? CompletedAtUtc { get; set; }

    // public ICollection<LessonProgressDto> LessonProgresses { get; set; } = new List<LessonProgressDto>();
    // public ICollection<LiveSessionAttendance> Attendances { get; set; } = new List<LiveSessionAttendance>();
}

public class EnrollmentResponseDto
{
    public Guid Id {get;set;}
    public Guid CourseId { get; set; }
    public EnrollmentStatus Status {get;set;}


}
public class EnrollmentConfirmDto
{
    public Guid Id {get;set;}
    public EnrollmentStatus Status {get;set;}


}
public class LessonProgressDto
{
    public Guid EnrollmentId { get; set; }
    public Guid LessonId { get; set; }
    public bool IsCompleted { get; set; }
    public int LastPositionSeconds { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
}

public class LiveSessionAttendance 
{
    public Guid EnrollmentId { get; set; }
    // public Enrollment Enrollment { get; set; } = default!;

    public Guid LiveSessionId { get; set; }
    public DateTime JoinedAtUtc { get; set; }
    public DateTime? LeftAtUtc { get; set; }
}
