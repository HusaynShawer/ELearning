using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ELearning.Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ELearning.Identity.Application.Service;

public class TokenService
{
    private readonly IConfiguration _config;
    private readonly UserManager<ApplicationUser> _userManager;

    public TokenService(
        IConfiguration configuration,
        UserManager<ApplicationUser> userManager)
    {
        _config = configuration;
        _userManager = userManager;
    }

    public async Task<string> GenerateTokenAsync(ApplicationUser user)
    {
        // 1. Create basic user claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!)
        };

        // 2. Get user roles from Identity
        var roles = await _userManager.GetRolesAsync(user);

        // 3. Add roles to JWT claims
        foreach (var role in roles)
        {
            claims.Add(new Claim("role", role));
        }

        // 4. Create signing key
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]!)
        );

        // 5. Create signing credentials
        var creds = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        // 6. Create JWT
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                double.Parse(_config["Jwt:AccessTokenExpirationMinutes"]!)
            ),
            signingCredentials: creds
        );

        // 7. Convert JWT to string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
