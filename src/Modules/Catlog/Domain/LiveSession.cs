using ELearning.SharedKernel;

namespace ELearning.Catalog.Domain;

public class LiveSession : BaseEntity
{
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = default!;

    public string Title { get; set; } = default!;
    public DateTime ScheduledStartUtc { get; set; }
    public int DurationMinutes { get; set; }
    public string? RoomId { get; set; }
    public LiveSessionStatus Status { get; set; } = LiveSessionStatus.Scheduled;
}