using ELearning.Identity.Application.Abstractions;
using ELearning.Identity.Domain;
using Microsoft.EntityFrameworkCore;

namespace ELearning.Identity.Infrastructure;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppIdentityDbContext _context;

    public RefreshTokenRepository(AppIdentityDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash)
    {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.TokenHash == tokenHash);
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
