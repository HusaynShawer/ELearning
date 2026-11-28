using System.ComponentModel.DataAnnotations;

namespace ELearning.Identity.Auth;

public class RefreshTokenRequestDto
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}

public class RefreshTokenLoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime TokenExpiresAtUtc { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiresAtUtc { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
}

public class RefreshTokenResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime TokenExpiresAtUtc { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiresAtUtc { get; set; }
}
