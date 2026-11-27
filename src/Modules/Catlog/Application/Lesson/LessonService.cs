using ELearning.Catalog.Application.Dtos;
using ELearning.Catalog.Domain;
using ELearning.Catalog.Infrastructure;
using ELearning.SharedKernel.Application.Exceptions;

namespace ELearning.Catalog.Application;

public class LessonService
{
    private readonly LessonRepository _lessonRepository;
    private readonly IVideoStorageService _localVideoStorageService;
    private readonly SectionRepository _sectionRepository;
    private readonly CourseRepository _courseRepository;

    private readonly UnitOfWork _uow;

    public LessonService(LessonRepository lessonRepository,
                        IVideoStorageService localVideoStorageService,
                        SectionRepository sectionRepository,
                        CourseRepository courseRepository,
                        UnitOfWork uow)
    {
        _lessonRepository = lessonRepository;
        _sectionRepository = sectionRepository;
        _courseRepository = courseRepository;
        _localVideoStorageService = localVideoStorageService;
        _uow = uow;
    }

    public async Task<LessonDto> AddAsync(LessonDto lessonDto, Guid ownerId)
    {
        var course = await GetCourseBySectionIdAsync(lessonDto.SectionId);
        if (course.InstructorId != ownerId)
            throw new ForbiddenException("You do not have permission to edit this lesson.");

        var lesson = new Lesson
        {
            SectionId = lessonDto.SectionId,
            Title = lessonDto.Title,
            Type = lessonDto.Type,
            ContentUrl = lessonDto.ContentUrl,
            DurationSeconds = lessonDto.DurationSeconds,
            Order = lessonDto.Order,
            IsFreePreview = lessonDto.IsFreePreview
        };

        await _lessonRepository.AddAsync(lesson);
        await _uow.SaveAsync();

        return new LessonDto
        {
            Id = lesson.Id,
            SectionId = lesson.SectionId,
            Title = lesson.Title,
            Type = lesson.Type,
            ContentUrl = lesson.ContentUrl,
            DurationSeconds = lesson.DurationSeconds,
            Order = lesson.Order,
            IsFreePreview = lesson.IsFreePreview
        };
    }

    public async Task<IEnumerable<LessonDto>> GetAllAsync(Guid sectionId)
    {
        var lessons = await _lessonRepository.GetAllAsync(sectionId);
        return lessons.Any() ? lessons : throw new NotFoundException("No lessons found for this section.");
    }

    public async Task UploadVideoAsync(
        Guid lessonId,
        Guid ownerId,
        IFormFile video,
        CancellationToken cancellationToken)
    {
        var lesson = await _lessonRepository.GetByIdAsync(lessonId);

        if (lesson == null)
            throw new NotFoundException("Lesson not found.");

        var course = await GetCourseByLessonIdAsync(lessonId);
        if (course.InstructorId != ownerId)
            throw new ForbiddenException("You do not have permission to upload a video for this lesson.");

        var videoKey = await _localVideoStorageService.UploadAsync(
            video,
            lessonId,
            cancellationToken
        );

        lesson.ContentUrl = videoKey;

        await _uow.SaveAsync();
    }

    public async Task<IEnumerable<LessonDto>> GetByCourseIdAsync(Guid courseId)
    {
        var sections = await _sectionRepository.GetAllAsync(courseId);
        var lessons = new List<LessonDto>();
        foreach (var section in sections)
        {
            var lesson = await _lessonRepository.GetAllAsync(section.Id);
            if (lesson is not null)
            {
                lessons.AddRange(lesson);
            }
        }
        return lessons;
    }

    public async Task<Course> GetCourseByLessonIdAsync(Guid lessonId)
    {
        var lesson = await _lessonRepository.GetByIdAsync(lessonId);
        if (lesson is not null)
        {
            var section = await _sectionRepository.GetByIdAsync(lesson.SectionId);
            if (section is not null)
            {
                var course = await _courseRepository.GetCourseByIdAsync(section.CourseId);
                return course ?? throw new NotFoundException("Lesson not found in this course.");
            }
        }
        throw new NotFoundException("Lesson not found.");
    }

    public async Task<Course> GetCourseBySectionIdAsync(Guid sectionId)
    {
        var section = await _sectionRepository.GetByIdAsync(sectionId);
        if (section is not null)
        {
            var course = await _courseRepository.GetCourseByIdAsync(section.CourseId);
            return course ?? throw new NotFoundException("Lesson not found in this course.");
        }
        throw new NotFoundException("The section does not belong to any course.");
    }
}
