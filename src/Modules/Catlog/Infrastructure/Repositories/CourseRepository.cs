using ELearning.Catalog.Domain;
using ELearning.Catalog.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ELearning.Catalog.Infrastructure;

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

    public async Task<Course?> GetCourseByIdAsync(Guid courseId)
    {
        return await _context.Courses
            .AsNoTracking()
            .AsSplitQuery()
            .Include(c => c.Sections.OrderBy(s => s.Order))
                .ThenInclude(s => s.Lessons.OrderBy(l => l.Order))
            .FirstOrDefaultAsync(c => c.Id == courseId);
    }

    public async Task<IEnumerable<Course>> GetCoursesByInstructorAsync(Guid instructorId)
    {
        return await _context.Courses
            .AsNoTracking()
            .Where(c => c.InstructorId == instructorId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Course>> GetCoursesByCategoryAsync(Guid categoryId)
    {
        return await _context.Courses
            .AsNoTracking()
            .Where(c => c.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Course>> GetAllCoursesAsync()
    {
        return await _context.Courses
            .AsNoTracking()
            .ToListAsync();
    }
}
