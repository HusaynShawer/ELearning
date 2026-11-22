using ELearning.Catalog.Application.Dtos;
using ELearning.Catalog.Domain;
using ELearning.Catalog.Infrastructure;

public class SectionService
{
    private readonly SectionRepository _sectionRepository;
    private readonly UnitOfWork _uow;

    public SectionService(SectionRepository sectionRepository,
                            UnitOfWork uow)
    {
     _sectionRepository = sectionRepository;
     _uow = uow;   
    }

    public async Task AddSection(SectionDto sectionDto)
    {
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
}