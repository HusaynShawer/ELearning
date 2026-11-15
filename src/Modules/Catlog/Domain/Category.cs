using ELearning.SharedKernel;
namespace ELearning.Catalog.Domain;

public class Category : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public Guid? ParentId { get; set; }

    public Category? Parent { get; set; }
    public ICollection<Category> Children { get; set; } = new List<Category>();
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}