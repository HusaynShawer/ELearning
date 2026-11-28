using System.Security.Cryptography;
using System.Text;
using ELearning.Identity.Auth;
using ELearning.Identity.Domain;
using ELearning.Identity.Infrastructure;
using ELearning.SharedKernel.Application.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ELearning.Identity.Application.Service;

public class RefreshTokenService
{
    private readonly AppIdentityDbContext _context;
    private readonly TokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _config;

    public RefreshTokenService(
        AppIdentityDbContext context,
        TokenService tokenService,
        UserManager<ApplicationUser> userManager,
        IConfiguration config)
    {
        _context = context;
        _tokenService = tokenService;
        _userManager = userManager;
        _config = config;
    }

    public async Task<RefreshTokenLoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.email);

        if (user is null || !user.IsActive)
            throw new UnauthorizedException("Invalid email or password.");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.password);

        if (!isPasswordValid)
            throw new UnauthorizedException("Invalid email or password.");

        var accessToken = await _tokenService.GenerateTokenAsync(user);
        var issued = await IssueRefreshTokenAsync(user.Id);

        return new RefreshTokenLoginResponseDto
        {
            Token = accessToken,
            TokenExpiresAtUtc = GetAccessTokenExpirationUtc(),
            RefreshToken = issued.PlainToken,
            RefreshTokenExpiresAtUtc = issued.ExpiresAtUtc,
            UserId = user.Id,
            Email = user.Email!,
            UserName = user.UserName!
        };
    }

    public async Task<RefreshTokenResponseDto> RefreshAsync(string refreshToken)
    {
        var tokenHash = HashToken(refreshToken);

        var storedToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.TokenHash == tokenHash);

        if (storedToken is null || !storedToken.IsActive)
            throw new UnauthorizedException("Invalid or expired refresh token.");

        var user = await _userManager.FindByIdAsync(storedToken.UserId.ToString());

        if (user is null || !user.IsActive)
            throw new UnauthorizedException("Invalid or expired refresh token.");

        var issued = CreateRefreshToken(user.Id);

        storedToken.RevokedAtUtc = DateTime.UtcNow;
        storedToken.ReplacedByTokenHash = issued.Token.TokenHash;

        _context.RefreshTokens.Add(issued.Token);
        await _context.SaveChangesAsync();

        var accessToken = await _tokenService.GenerateTokenAsync(user);

        return new RefreshTokenResponseDto
        {
            Token = accessToken,
            TokenExpiresAtUtc = GetAccessTokenExpirationUtc(),
            RefreshToken = issued.PlainToken,
            RefreshTokenExpiresAtUtc = issued.ExpiresAtUtc
        };
    }

    public async Task RevokeAsync(string refreshToken)
    {
        var tokenHash = HashToken(refreshToken);

        var storedToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.TokenHash == tokenHash);

        if (storedToken is null || storedToken.RevokedAtUtc is not null)
            return;

        storedToken.RevokedAtUtc = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    private async Task<IssuedRefreshToken> IssueRefreshTokenAsync(Guid userId)
    {
        var issued = CreateRefreshToken(userId);

        _context.RefreshTokens.Add(issued.Token);
        await _context.SaveChangesAsync();

        return issued;
    }

    private IssuedRefreshToken CreateRefreshToken(Guid userId)
    {
        var plainToken = GenerateSecureToken();
        var expiresAtUtc = DateTime.UtcNow.AddDays(GetRefreshTokenExpirationDays());

        var token = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TokenHash = HashToken(plainToken),
            CreatedAtUtc = DateTime.UtcNow,
            ExpiresAtUtc = expiresAtUtc
        };

        return new IssuedRefreshToken(token, plainToken, expiresAtUtc);
    }

    private DateTime GetAccessTokenExpirationUtc()
    {
        return DateTime.UtcNow.AddMinutes(
            double.Parse(_config["Jwt:AccessTokenExpirationMinutes"]!));
    }

    private double GetRefreshTokenExpirationDays()
    {
        var value = _config["Jwt:RefreshTokenExpirationDays"];

        return double.TryParse(value, out var days) ? days : 7;
    }

    private static string GenerateSecureToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(bytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');
    }

    private static string HashToken(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));

        return Convert.ToHexString(bytes);
    }

    private sealed record IssuedRefreshToken(
        RefreshToken Token,
        string PlainToken,
        DateTime ExpiresAtUtc);
}
