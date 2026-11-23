
namespace ELearning.Catalog.Application;

public class LocalVideoStorgeService : IVideoStorageService
{
    private readonly string _storagePath;
    private readonly SectionService _sectionService;
    private readonly CourseService _courseService;
    private readonly LessonRepository _lessonRepository;

    public LocalVideoStorgeService(
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
            throw new ArgumentException("Invalid video file.");

        var extension = Path.GetExtension(video.FileName).ToLowerInvariant();

        var allowedExtensions = new[] { ".mp4", ".webm", ".mov" };

        if (!allowedExtensions.Contains(extension))
            throw new ArgumentException("Unsupported video format.");

        var relativePath = await LessonSavePath(lessonId);

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
            throw new ArgumentException("Invalid video key.");

        if (File.Exists(filePath))
            File.Delete(filePath);

        await Task.CompletedTask;
    }

    public async Task<string> LessonSavePath(Guid lessonId)
    {
        var lesson = await _lessonRepository.GetByIdAsync(lessonId);

        if (lesson == null)
            throw new Exception("Lesson not found.");

        var section = await _sectionService.GetById(lesson.SectionId);

        if (section == null)
            throw new Exception("Section not found.");

        var course = await _courseService.GetByIdAsync(section.CourseId);

        if (course == null)
            throw new Exception("Course not found.");

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
