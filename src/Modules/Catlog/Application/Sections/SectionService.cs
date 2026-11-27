using ELearning.Catalog.Application.Dtos;
using ELearning.Catalog.Domain;
using ELearning.Catalog.Infrastructure;
using ELearning.SharedKernel.Application.Exceptions;

namespace ELearning.Catalog.Application;

public class SectionService
{
    private readonly SectionRepository _sectionRepository;
    private readonly CourseService _courseService;

    private readonly UnitOfWork _uow;

    public SectionService(SectionRepository sectionRepository,
                            CourseService courseService,
                            UnitOfWork uow)
    {
        _sectionRepository = sectionRepository;
        _courseService = courseService;
        _uow = uow;
    }

    public async Task AddSectionAsync(SectionDto sectionDto, Guid ownerId)
    {
        var course = await _courseService.GetByIdAsync(sectionDto.CourseId);
        if (course is null)
            throw new NotFoundException("Course not found.");

        if (course.InstructorId != ownerId)
            throw new ForbiddenException("You do not have permission to add a section to this course.");

        var section = new Section
        {
            CourseId = sectionDto.CourseId,
            Title = sectionDto.Title,
            Order = sectionDto.Order,
            Lessons = sectionDto.Lessons.Select(l => new Lesson
            {
                Title = l.Title,
                Type = l.Type,
                ContentUrl = l.ContentUrl,
                DurationSeconds = l.DurationSeconds,
                Order = l.Order,
                IsFreePreview = l.IsFreePreview
            }).ToList()
        };

        await _sectionRepository.AddAsync(section);
        await _uow.SaveAsync();
    }

    public async Task<IEnumerable<SectionDto>> GetAllAsync(Guid courseId)
    {
        var sections = await _sectionRepository.GetAllAsync(courseId);
        return sections.Any() ? sections : throw new NotFoundException("No sections found for this course.");
    }

    public async Task<Section> GetByIdAsync(Guid sectionId)
    {
        var section = await _sectionRepository.GetByIdAsync(sectionId);
        if (section is null)
        {
            throw new NotFoundException("Section not found.");
        }
        return section;
    }
}
