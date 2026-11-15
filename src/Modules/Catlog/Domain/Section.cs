using ELearning.SharedKernel;

namespace ELearning.Catalog.Domain;

public class Section : BaseEntity
{
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = default!;

    public string Title { get; set; } = default!;
    public int Order { get; set; }

    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
