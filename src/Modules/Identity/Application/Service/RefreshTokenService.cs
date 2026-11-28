using ELearning.Identity.Application.Abstractions;
using ELearning.Identity.Auth;
using ELearning.Identity.Domain;
using ELearning.SharedKernel.Application.Exceptions;

namespace ELearning.Identity.Application.Service;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IRefreshTokenRepository _repository;
    private readonly IRefreshTokenGenerator _generator;
    private readonly IUserCredentialValidator _credentialValidator;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _config;

    public RefreshTokenService(
        IRefreshTokenRepository repository,
        IRefreshTokenGenerator generator,
        IUserCredentialValidator credentialValidator,
        ITokenService tokenService,
        IConfiguration config)
    {
        _repository = repository;
        _generator = generator;
        _credentialValidator = credentialValidator;
        _tokenService = tokenService;
        _config = config;
    }

    public async Task<RefreshTokenLoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _credentialValidator.ValidateCredentialsAsync(
            loginDto.email,
            loginDto.password);

        var accessToken = await _tokenService.GenerateTokenAsync(user);
        var issued = CreateRefreshToken(user.Id);

        await _repository.AddAsync(issued.Token);
        await _repository.SaveChangesAsync();

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
        var storedToken = await _repository.GetByTokenHashAsync(
            _generator.HashToken(refreshToken));

        if (storedToken is null || !storedToken.IsActive)
            throw new UnauthorizedException("Invalid or expired refresh token.");

        var user = await _credentialValidator.GetActiveUserByIdAsync(storedToken.UserId);
        var issued = CreateRefreshToken(user.Id);

        storedToken.RevokedAtUtc = DateTime.UtcNow;
        storedToken.ReplacedByTokenHash = issued.Token.TokenHash;

        await _repository.AddAsync(issued.Token);
        await _repository.SaveChangesAsync();

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
        var storedToken = await _repository.GetByTokenHashAsync(
            _generator.HashToken(refreshToken));

        if (storedToken is null || storedToken.RevokedAtUtc is not null)
            return;

        storedToken.RevokedAtUtc = DateTime.UtcNow;
        await _repository.SaveChangesAsync();
    }

    private IssuedRefreshToken CreateRefreshToken(Guid userId)
    {
        var plainToken = _generator.GenerateToken();
        var expiresAtUtc = DateTime.UtcNow.AddDays(GetRefreshTokenExpirationDays());

        var token = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TokenHash = _generator.HashToken(plainToken),
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

    private sealed record IssuedRefreshToken(
        RefreshToken Token,
        string PlainToken,
        DateTime ExpiresAtUtc);
}
