using ELearning.Identity.Auth;

namespace ELearning.Identity.Application.Abstractions;

public interface IRefreshTokenService
{
    Task<RefreshTokenLoginResponseDto> LoginAsync(LoginDto loginDto);
    Task<RefreshTokenResponseDto> RefreshAsync(string refreshToken);
    Task RevokeAsync(string refreshToken);
}
