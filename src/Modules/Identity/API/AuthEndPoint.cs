using ELearning.Identity.Auth;
using ELearning.Identity.Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthEndPoint : ControllerBase
{
    private readonly AuthService _service;
    public AuthEndPoint(AuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        var reg = await _service.RegisterAsync(dto);
        return Ok(reg);
    }

    [HttpPost("login")]
    public async Task<ActionResult> LogIn([FromBody] LoginDto dto)
    {
        var login = await _service.LoginAsync(dto);
        return Ok(login);
    }
}
