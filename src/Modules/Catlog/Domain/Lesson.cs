using ELearning.SharedKernel;

namespace ELearning.Catalog.Domain;

public class Lesson : BaseEntity
{
    public Guid SectionId { get; set; }
    public Section Section { get; set; } = default!;

    public string Title { get; set; } = default!;
    public LessonType Type { get; set; }
    public string? ContentUrl { get; set; }
    public int DurationSeconds { get; set; }
    public int Order { get; set; }
    public bool IsFreePreview { get; set; } = false;
}