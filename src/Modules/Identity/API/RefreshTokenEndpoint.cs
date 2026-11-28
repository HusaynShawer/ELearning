using ELearning.Identity.Application.Abstractions;
using ELearning.Identity.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class RefreshTokenEndpoint : ControllerBase
{
    private readonly IRefreshTokenService _service;

    public RefreshTokenEndpoint(IRefreshTokenService service)
    {
        _service = service;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginDto dto)
    {
        var login = await _service.LoginAsync(dto);
        return Ok(login);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult> Refresh([FromBody] RefreshTokenRequestDto dto)
    {
        var tokens = await _service.RefreshAsync(dto.RefreshToken);
        return Ok(tokens);
    }

    [HttpPost("revoke")]
    public async Task<ActionResult> Revoke([FromBody] RefreshTokenRequestDto dto)
    {
        await _service.RevokeAsync(dto.RefreshToken);
        return NoContent();
    }
}
