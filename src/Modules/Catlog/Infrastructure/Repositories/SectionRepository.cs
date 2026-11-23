using ELearning.Catalog.Application.Dtos;
using ELearning.Catalog.Domain;
using ELearning.Catalog.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
    public async Task<IEnumerable<SectionDto>> GetAll(Guid courseId)
    {
        var sections = await _context.Sections.Where(s =>s.CourseId == courseId).ToListAsync();
        return sections.Select(s => new SectionDto
        {
            Id = s.Id,
            CourseId = s.CourseId,
            Title = s.Title,
            Order = s.Order,
            Lessons = s.Lessons.Select(l => new LessonWithSectionDto
            {
                Type  = l.Type,
                ContentUrl = l.ContentUrl,
                DurationSeconds = l.DurationSeconds,
                Order = l.Order,
                IsFreePreview = l.IsFreePreview
            }).ToList()
        }).ToList();
    }

    public async Task<Section?> GetById(Guid sectionId)
    {
        return await _context.Sections.FindAsync(sectionId);
    }
}