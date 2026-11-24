using ELearning.Catalog.Application.Dtos;
using ELearning.Catalog.Domain;
using ELearning.Catalog.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;

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

    public async Task AddSection(SectionDto sectionDto, Guid ownerId)
    {
        var course = await _courseService.GetByIdAsync(sectionDto.CourseId);
        if (course is null)
            throw new Exception("course not found");
        
        if (course.InstructorId != ownerId)
                throw new Exception("you dont have the right to upload to this course");

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

    public async Task<IEnumerable<SectionDto>> GetAll(Guid courseId)
    {
        var sections = await _sectionRepository.GetAll(courseId);
        return sections.Any() ? sections : throw new Exception("no section in this course");
    }

    public async Task<Section> GetById(Guid sectionId)
    {
        var section =  await _sectionRepository.GetById(sectionId);
        if (section is null)
        {
            throw new Exception("no section found");
        }
        return section;
    }
}