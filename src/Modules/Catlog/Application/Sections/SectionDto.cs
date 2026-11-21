namespace ELearning.Catalog.Application.Dtos;
public class SectionWithCourseDto
{

    public string Title { get; set; } = default!;
    public int Order { get; set; }

    public ICollection<LessonWithSectionDto> Lessons { get; set; } = new List<LessonWithSectionDto>(); 
}

public class SectionDto
{
    public Guid CourseId { get; set; }

    public string Title { get; set; } = default!;
    public int Order { get; set; }

    public ICollection<LessonDto> Lessons { get; set; } = new List<LessonDto>(); 
}