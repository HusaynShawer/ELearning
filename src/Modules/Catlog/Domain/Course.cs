using ELearning.SharedKernel;
namespace ELearning.Catalog.Domain;

public class Course : BaseEntity
{
    public string Title {get; set;} = default!;
    public string Slug {get; set;} = default!;
    public string Description { get; set; } = default!;
    public string? ThumbnailUrl { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "EGP";
    public CourseLevel Level { get; set; }
    public CourseStatus Status { get; set; } = CourseStatus.Draft;
    public Guid InstructorId { get; set; }

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = default!;

    public ICollection<Section> Sections { get; set; } = new List<Section>();
    public ICollection<LiveSession> LiveSessions { get; set; } = new List<LiveSession>();

}