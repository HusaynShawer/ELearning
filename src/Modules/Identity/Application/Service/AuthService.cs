
using ELearning.Identity.Auth;
using ELearning.Identity.Domain;
using Microsoft.AspNetCore.Identity;

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

    public async Task<RegisterResponseDto> Register(RegisterRequestDto request)
    {
        var existedUser = await _userManager.FindByEmailAsync(request.Email);

        if (existedUser is not null)
            throw new InvalidOperationException("Email already exists");

        var existedUserName = await _userManager.FindByNameAsync(request.UserName);

        if (existedUserName is not null)
            throw new InvalidOperationException("Username already exists");

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
            throw new InvalidOperationException(errors);
        }

        var roleResult = await _userManager.AddToRoleAsync(user, Roles.Student);

        if (!roleResult.Succeeded)
        {
            var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
            throw new InvalidOperationException(errors);
        }

        return new RegisterResponseDto
        {
            UserId = user.Id
        };
    }

    public async Task<LoginResponseDto> LogIn(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.email);

        if (user is null || !user.IsActive)
            throw new UnauthorizedAccessException("Invalid email or password");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.password);

        if (!isPasswordValid)
            throw new UnauthorizedAccessException("Invalid email or password");

        var token = await _tokenService.GenerateToken(user);

        return new LoginResponseDto
        {
            Token = token,
            UserId = user.Id,
            Email = user.Email!,
            UserName = user.UserName!
        };
    }
}
