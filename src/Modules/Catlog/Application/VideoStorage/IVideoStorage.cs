namespace ELearning.Catalog.Application;
public interface IVideoStorageService
{
    Task<string> UploadAsync(
        IFormFile video,
        Guid lessionId,
        CancellationToken cancellationToken = default
    );

    Task DeleteAsync(
        string videoKey,
        CancellationToken cancellationToken = default
    );
}