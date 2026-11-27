using ELearning.Identity.Auth;
using ELearning.Identity.Domain;
using ELearning.SharedKernel.Application.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace ELearning.Identity.Application.Service;

public class AuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly TokenService _tokenService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        TokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        var existedUser = await _userManager.FindByEmailAsync(request.Email);

        if (existedUser is not null)
            throw new ConflictException("Email already exists.");

        var existedUserName = await _userManager.FindByNameAsync(request.UserName);

        if (existedUserName is not null)
            throw new ConflictException("Username already exists.");

        // Create user
        var user = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email,
            FullName = request.FullName,
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ValidationException(errors);
        }

        var roleResult = await _userManager.AddToRoleAsync(user, Roles.Student);

        if (!roleResult.Succeeded)
        {
            var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
            throw new ValidationException(errors);
        }

        return new RegisterResponseDto
        {
            UserId = user.Id
        };
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.email);

        if (user is null || !user.IsActive)
            throw new UnauthorizedException("Invalid email or password.");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.password);

        if (!isPasswordValid)
            throw new UnauthorizedException("Invalid email or password.");

        var token = await _tokenService.GenerateTokenAsync(user);

        return new LoginResponseDto
        {
            Token = token,
            UserId = user.Id,
            Email = user.Email!,
            UserName = user.UserName!
        };
    }
}
