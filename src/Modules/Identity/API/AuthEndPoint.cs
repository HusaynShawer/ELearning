using ELearning.Identity.Auth;
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
        var reg = await _service.Register(dto);
        return Ok(reg);
    }

    [HttpPost("login")]
    public async Task<ActionResult> LogIn(string email, string password)
    {
        var login = await _service.LogIn(email, password);
        return Ok(login);
    }
}