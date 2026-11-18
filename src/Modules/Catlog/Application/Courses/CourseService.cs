using System.Collections.Immutable;
using ELearning.Catalog.Application;
using ELearning.Catalog.Domain;
using ELearning.Catalog.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;

public class CourseService
{
    private readonly CourseRepository _courseRepo;
    private readonly UnitOfWork _uow;
    public CourseService(CourseRepository courseRepository,
                            UnitOfWork uow)
    {
        _courseRepo = courseRepository;
        _uow = uow;
    }

    public async Task<CourseResponeDto> CreateAsync(CourseRequestDto dto)
    {
        var course = new Course
        {
            Title = dto.Title,
            Slug = dto.Slug,
            Description = dto.Description,
            ThumbnailUrl = dto.ThumbnailUrl,
            Price = dto.Price,
            Currency = "EGP",
            Level = dto.Level,
            Status = dto.Status,
            CategoryId = dto.CategoryId,
            InstructorId = dto.InstructorId, // <-- must set this

            Sections = dto.Sections.Select(s => new Section
            {
                Title = s.Title,
                Order = s.Order,
                Lessons = s.Lessons.Select(l => new Lesson
                {
                    Title = l.Title,
                    Order = l.Order,
                    Type = l.Type,
                    ContentUrl = l.ContentUrl,
                    DurationSeconds = l.DurationSeconds,
                }).ToList()
            }).ToList()
        };

        await _uow.SaveAsync();
        return new CourseResponeDto
        {
            CourseId     = course.Id,
            Title        = course.Title,
            Description  = course.Description,
            ThumbnailUrl = course.ThumbnailUrl,
            Price        = course.Price,
            Level        = course.Level,
            CategoryId   = course.CategoryId,
            InstructorId = course.InstructorId,

            Sections = course.Sections.Select(s => new SectionDto
            {
                Title = s.Title,
                Order = s.Order,
                Lessons = s.Lessons.Select(l => new LessonDto
                {
                    Title           = l.Title,
                    Order           = l.Order,
                    Type            = l.Type,
                    ContentUrl      = l.ContentUrl,
                    DurationSeconds = l.DurationSeconds,
                }).ToList()
            }).ToList()
        };
    }
}