using ELearning.Identity.Domain;

namespace ELearning.Identity.Application.Abstractions;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(ApplicationUser user);
}
