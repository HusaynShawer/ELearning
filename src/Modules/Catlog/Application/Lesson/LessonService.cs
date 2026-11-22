using System.Runtime.CompilerServices;
using ELearning.Catalog.Application;
using ELearning.Catalog.Application.Dtos;
using ELearning.Catalog.Domain;
using ELearning.Catalog.Infrastructure;

public class LessonService
{
    private readonly LessonRepository _lessonRepository;
    private readonly IVideoStorageService _localVideoStorgeService;
    private readonly UnitOfWork _uow;

    public LessonService(LessonRepository lessonRepository,
                        IVideoStorageService localVideoStorgeService,
                        UnitOfWork uow)    
    {
        _lessonRepository = lessonRepository;
        _localVideoStorgeService = localVideoStorgeService;
        _uow = uow;
    }

    public async Task<LessonDto> Add(LessonDto lessonDto)
    {
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
        return lessonDto;
    }

    public async Task<IEnumerable<LessonDto>> GetAll(Guid sectionId)
    {
        var lessons = await _lessonRepository.GetAllAsync(sectionId);
        return lessons.Any() ? lessons : throw new Exception("no leesons in this section try to add one!");
    }
    public async Task UploadVideoAsync(
        Guid lessonId,
        IFormFile video,
        CancellationToken cancellationToken)
    {
        var lesson = await _lessonRepository.GetByIdAsync(lessonId);

        if (lesson == null)
            throw new Exception("Lesson not found.");

        var videoKey = await _localVideoStorgeService.UploadAsync(
            video,
            cancellationToken
        );

        // lesson.VideoKey = videoKey;

        await _uow.SaveAsync();
    }
    
}