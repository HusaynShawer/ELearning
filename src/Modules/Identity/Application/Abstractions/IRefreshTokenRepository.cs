using ELearning.Identity.Domain;

namespace ELearning.Identity.Application.Abstractions;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenHashAsync(string tokenHash);
    Task AddAsync(RefreshToken refreshToken);
    Task SaveChangesAsync();
}
