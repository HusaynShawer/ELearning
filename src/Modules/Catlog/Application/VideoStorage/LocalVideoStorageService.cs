using ELearning.Catalog.Infrastructure;
using ELearning.SharedKernel.Application.Exceptions;

namespace ELearning.Catalog.Application;

public class LocalVideoStorageService : IVideoStorageService
{
    private readonly string _storagePath;
    private readonly SectionService _sectionService;
    private readonly CourseService _courseService;
    private readonly LessonRepository _lessonRepository;

    public LocalVideoStorageService(
        IWebHostEnvironment environment,
        SectionService sectionService,
        CourseService courseService,
        LessonRepository lessonRepository)
    {
        _storagePath = Path.Combine(
            environment.ContentRootPath,
            "Storage",
            "Videos"
        );

        Directory.CreateDirectory(_storagePath);

        _sectionService = sectionService;
        _courseService = courseService;
        _lessonRepository = lessonRepository;
    }

    public async Task<string> UploadAsync(
        IFormFile video,
        Guid lessonId,
        CancellationToken cancellationToken = default)
    {
        if (video == null || video.Length == 0)
            throw new ValidationException("Invalid video file.");

        var extension = Path.GetExtension(video.FileName).ToLowerInvariant();

        var allowedExtensions = new[] { ".mp4", ".webm", ".mov" };

        if (!allowedExtensions.Contains(extension))
            throw new ValidationException("Unsupported video format.");

        var relativePath = await GetLessonSavePathAsync(lessonId);

        var directoryPath = Path.Combine(_storagePath, relativePath);

        Directory.CreateDirectory(directoryPath);

        var fileName = $"{Guid.NewGuid()}{extension}";

        var filePath = Path.Combine(directoryPath, fileName);

        await using var stream = new FileStream(
            filePath,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            81920,
            useAsync: true
        );

        await video.CopyToAsync(stream, cancellationToken);

        return Path.Combine(relativePath, fileName).Replace("\\", "/");
    }

    public async Task DeleteAsync(
        string videoKey,
        CancellationToken cancellationToken = default)
    {
        var filePath = Path.GetFullPath(
            Path.Combine(_storagePath, videoKey)
        );

        // Prevent accessing files outside storage directory
        var rootPath = Path.GetFullPath(_storagePath)
            + Path.DirectorySeparatorChar;

        if (!filePath.StartsWith(rootPath, StringComparison.OrdinalIgnoreCase))
            throw new ValidationException("Invalid video key.");

        if (File.Exists(filePath))
            File.Delete(filePath);

        await Task.CompletedTask;
    }

    public async Task<string> GetLessonSavePathAsync(Guid lessonId)
    {
        var lesson = await _lessonRepository.GetByIdAsync(lessonId);

        if (lesson == null)
            throw new NotFoundException("Lesson not found.");

        var section = await _sectionService.GetByIdAsync(lesson.SectionId);

        if (section == null)
            throw new NotFoundException("Section not found.");

        var course = await _courseService.GetByIdAsync(section.CourseId);

        if (course == null)
            throw new NotFoundException("Course not found.");

        return Path.Combine(
            MakeSafeFolderName(course.Title),
            MakeSafeFolderName(section.Title)
        );
    }

    private static string MakeSafeFolderName(string name)
    {
        var invalidChars = Path.GetInvalidFileNameChars();

        return string.Join("_",
            name.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries)
        ).Trim();
    }
}
