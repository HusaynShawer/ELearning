using ELearning.Catalog.Application.Dtos;
using ELearning.Catalog.Domain;
using Microsoft.EntityFrameworkCore;

namespace ELearning.Catalog.Infrastructure;

public class SectionRepository
{
    private readonly CatalogDbContext _context;

    public SectionRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Section section)
    {
        await _context.Sections.AddAsync(section);
    }

    public async Task<IEnumerable<SectionDto>> GetAllAsync(Guid courseId)
    {
        var sections = await _context.Sections
            .Include(s => s.Course)
            .Include(s => s.Lessons)
            .Where(s => s.CourseId == courseId)
            .OrderBy(s => s.Order)
            .ToListAsync();

        return sections.Select(s => new SectionDto
        {
            Id = s.Id,
            CourseId = s.CourseId,
            Title = s.Title,
            Order = s.Order,
            Lessons = (s.Lessons ?? Enumerable.Empty<Lesson>())
                .OrderBy(l => l.Order)
                .Select(l => new LessonWithSectionDto
                {
                    Type = l.Type,
                    DurationSeconds = l.DurationSeconds,
                    Order = l.Order,
                    IsFreePreview = l.IsFreePreview
                })
                .ToList()
        }).ToList();
    }

    public async Task<Section?> GetByIdAsync(Guid sectionId)
    {
        return await _context.Sections.FindAsync(sectionId);
    }
}
