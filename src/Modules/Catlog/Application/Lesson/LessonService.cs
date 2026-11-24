using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ELearning.Catalog.Application;
using ELearning.Catalog.Application.Dtos;
using ELearning.Catalog.Domain;
using ELearning.Catalog.Infrastructure;

public class LessonService
{
    private readonly LessonRepository _lessonRepository;
    private readonly IVideoStorageService _localVideoStorgeService;
    private readonly SectionRepository _sectionRepository;
    private readonly CourseRepository _courseRepository;

    private readonly UnitOfWork _uow;

    public LessonService(LessonRepository lessonRepository,
                        IVideoStorageService localVideoStorgeService,
                        SectionRepository sectionRepository,
                        CourseRepository courseRepository,
                        UnitOfWork uow)    
    {
        _lessonRepository = lessonRepository;
        _sectionRepository = sectionRepository;
        _courseRepository = courseRepository;
        _localVideoStorgeService = localVideoStorgeService;
        _uow = uow;
    }

    public async Task<LessonDto> Add(LessonDto lessonDto,Guid ownerId)
    {
        var course = await GetCourseBySectionId(lessonDto.SectionId);
        if (course.InstructorId != ownerId)
            throw new Exception("sorry you dont have rights to edit in the lesson");
        
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

    public async Task<IEnumerable<LessonDto>> GetAll(Guid sectionId)
    {
        var lessons = await _lessonRepository.GetAllAsync(sectionId);
        return lessons.Any() ? lessons : throw new Exception("no leesons in this section try to add one!");
    }
    public async Task UploadVideoAsync(
        Guid lessonId,
        Guid ownerId,
        IFormFile video,
        CancellationToken cancellationToken)
    {
        var lesson = await _lessonRepository.GetByIdAsync(lessonId);

        if (lesson == null)
            throw new Exception("Lesson not found.");

        var course = await GetCourseByLessonId(lessonId);
        if(course.InstructorId != ownerId)
            throw new Exception("you have no rights to upload video");

        var videoKey = await _localVideoStorgeService.UploadAsync(
            video,
            lessonId,
            cancellationToken
        );

        lesson.ContentUrl = videoKey;

        await _uow.SaveAsync();
    }

    public async Task<IEnumerable<LessonDto>> GetByCourseId(Guid courseId)
    {
        var sections = await _sectionRepository.GetAll(courseId);
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

    public async Task<Course> GetCourseByLessonId(Guid lessonId)
    {
        var lesson = await _lessonRepository.GetByIdAsync(lessonId);
        if (lesson is not null)
        {
            var section = await _sectionRepository.GetById(lesson.SectionId);
            if (section is not null)
            {
                var course = await _courseRepository.GetCourseByIdAsync(section.CourseId);
                return course ?? throw new Exception("lesson not in this course");
            }
        }
        throw new Exception("Lesson not found");

    }

    public async Task<Course> GetCourseBySectionId(Guid sectionId)
    {

            var section = await _sectionRepository.GetById(sectionId);
            if (section is not null)
            {
                var course = await _courseRepository.GetCourseByIdAsync(section.CourseId);
                return course ?? throw new Exception("lesson not in this course");
            }
            throw new Exception("the section dosent belong to any course");

    }
}