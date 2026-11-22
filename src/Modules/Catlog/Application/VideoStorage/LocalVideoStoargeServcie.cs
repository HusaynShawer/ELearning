namespace ELearning.Catalog.Application;

public class LocalVideoStorgeService : IVideoStorageService
{

    private readonly string _storagePath;
    public LocalVideoStorgeService(IWebHostEnvironment environment)
    {
        _storagePath = Path.Combine(
            environment.ContentRootPath,
            "Storge",
            "Videos"
        );
        Directory.CreateDirectory(_storagePath);
    }
    public Task DeleteAsync(string videoKey, CancellationToken cancellationToken = default)
    {
        var fileName = Path.GetFileName(videoKey);

        var filePath = Path.Combine(_storagePath, fileName);

        if (File.Exists(filePath))
            File.Delete(filePath);

        return Task.CompletedTask;
    }

    public async Task<string> UploadAsync(IFormFile video, CancellationToken cancellationToken = default)
    {
        if(video == null || video.Length == 0 )
            throw new ArgumentException("Invaled Video File");
        
        var extension = Path.GetExtension(video.FileName).ToLowerInvariant();
        var allowedExtensions = new[] { ".mp4", ".webm", ".mov" };
        if (!allowedExtensions.Contains(extension))
        throw new ArgumentException("Unsupported video format.");
        
        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(_storagePath, fileName);
       
        await using var stream = new FileStream(
            filePath,
            FileMode.CreateNew
        );

        await video.CopyToAsync(stream, cancellationToken);

        return $"Videos/{fileName}";
    }
}