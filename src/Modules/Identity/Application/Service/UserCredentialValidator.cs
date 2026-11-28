using ELearning.Identity.Application.Abstractions;
using ELearning.Identity.Domain;
using ELearning.SharedKernel.Application.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace ELearning.Identity.Application.Service;

public class UserCredentialValidator : IUserCredentialValidator
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserCredentialValidator(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationUser> ValidateCredentialsAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null || !user.IsActive)
            throw new UnauthorizedException("Invalid email or password.");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);

        if (!isPasswordValid)
            throw new UnauthorizedException("Invalid email or password.");

        return user;
    }

    public async Task<ApplicationUser> GetActiveUserByIdAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null || !user.IsActive)
            throw new UnauthorizedException("Invalid or expired refresh token.");

        return user;
    }
}
