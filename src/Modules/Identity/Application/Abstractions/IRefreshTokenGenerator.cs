namespace ELearning.Identity.Application.Abstractions;

public interface IRefreshTokenGenerator
{
    string GenerateToken();
    string HashToken(string token);
}
