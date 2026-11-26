using System.Security.Claims;
using ELearning.Catalog.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SectionController :ControllerBase
{
    private readonly SectionService _sectionService;
    public SectionController(SectionService sectionService)
    {
        _sectionService = sectionService;
    }
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<SectionDto>>> GetAll(Guid courseId)
    {
        
        var courses = await _sectionService.GetAll(courseId);
        return Ok(courses);
    }
    
    [HttpPost("")]
    [Authorize(Policy = "InstructorOrAdmin")]
    public async Task<ActionResult> AddSection(SectionDto sectionDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(!Guid.TryParse(userId,out var ownerId))
            return Forbid();

        await _sectionService.AddSection(sectionDto,ownerId);
        return Ok();
    }
}