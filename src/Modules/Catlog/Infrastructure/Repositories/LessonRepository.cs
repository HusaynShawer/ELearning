using ELearning.Catalog.Application.Dtos;
using ELearning.Catalog.Domain;
using ELearning.Catalog.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class LessonRepository
{
    private readonly CatalogDbContext _context;
    public LessonRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Lesson lesson)
    {
        await _context.Lessons.AddAsync(lesson);
    }
    public async Task<IEnumerable<LessonDto>> GetAllAsync(Guid sectionId)
    {
        var lessons = await _context.Lessons.Where(l =>l.SectionId ==sectionId).ToListAsync();
        var lessonsDto = lessons.Select(l => new LessonDto
        {
            Id = l.Id,
            SectionId = l.SectionId,
            Title = l.Title,
            Type = l.Type,
            ContentUrl = l.ContentUrl,
            DurationSeconds =l.DurationSeconds,
            Order = l.Order,
            IsFreePreview= l.IsFreePreview
        }).ToList();
        return lessonsDto.Any() ? lessonsDto : throw new Exception("no lessons with this section");
    }
    public async Task<Lesson?> GetByIdAsync(Guid Id)
    {
        return await _context.Lessons.FindAsync(Id);
    }
}