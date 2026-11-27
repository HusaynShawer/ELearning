using ELearning.Catalog.Domain;
namespace ELearning.Catalog.Application.Dtos;

public class CourseRequestDto
{
    public string Title {get; set;}
    public string Slug {get; set;}
    public string Description {get; set;}
    public string? ThumbnailUrl { get; set; }
    public decimal Price { get; set; }
    public CourseLevel Level { get; set; }
    public CourseStatus Status { get; set; } = CourseStatus.Draft;
    public Guid InstructorId { get; set; }
    public Guid CategoryId { get; set; }
    public ICollection<SectionWithCourseDto> Sections {get; set;} = new List<SectionWithCourseDto>();

}

public class CourseResponseDto
{
    public Guid CourseId {get; set;}
    public string Title {get;set;}
    public string Description {get; set;}
    public string ThumbnailUrl {get; set;}
    public decimal Price {get;set;}
    public CourseLevel Level {get;set;}
    public Guid InstructorId { get; set; }
    public Guid CategoryId { get; set; }
    public ICollection<SectionWithCourseDto> Sections {get; set;} = new List<SectionWithCourseDto>();
}