namespace ELearning.Catalog.Application;
public class SectionDto
{
    public Guid CourseId { get; set; }

    public string Title { get; set; } = default!;
    public int Order { get; set; }

    public ICollection<LessonDto> Lessons { get; set; } = new List<LessonDto>(); 
}