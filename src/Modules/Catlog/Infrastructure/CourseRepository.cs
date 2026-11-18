using ELearning.Catalog.Domain;
using ELearning.Catalog.Infrastructure;
using Microsoft.EntityFrameworkCore;

public class CourseRepository
{
    private readonly CatalogDbContext _context;
    public CourseRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task AddCourseAsync(Course course)
    {
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
    }
    public async Task<Course?> GetCourseAsync(Guid courseId)
    {
        return await _context.Courses.FindAsync(courseId);
        
    }

    public async Task<IEnumerable<Course>> GetCoursesByInstructor(Guid instructorId)
    {
        return await _context.Courses.Where(c => c.InstructorId == instructorId).ToListAsync();
    }
}