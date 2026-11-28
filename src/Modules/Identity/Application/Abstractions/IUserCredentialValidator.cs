using ELearning.Identity.Domain;

namespace ELearning.Identity.Application.Abstractions;

public interface IUserCredentialValidator
{
    Task<ApplicationUser> ValidateCredentialsAsync(string email, string password);
    Task<ApplicationUser> GetActiveUserByIdAsync(Guid userId);
}
