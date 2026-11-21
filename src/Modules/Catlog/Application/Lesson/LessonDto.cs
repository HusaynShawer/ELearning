using ELearning.Catalog.Domain;
namespace ELearning.Catalog.Application.Dtos;

public class LessonWithSectionDto
{

    public string Title { get; set; } = default!;
    public LessonType Type { get; set; }
    public string? ContentUrl { get; set; }
    public int DurationSeconds { get; set; }
    public int Order { get; set; }
    public bool IsFreePreview { get; set; }
}

public class LessonDto
{
    public Guid SectionId { get; set; }

    public string Title { get; set; } = default!;
    public LessonType Type { get; set; }
    public string? ContentUrl { get; set; }
    public int DurationSeconds { get; set; }
    public int Order { get; set; }
    public bool IsFreePreview { get; set; }
}