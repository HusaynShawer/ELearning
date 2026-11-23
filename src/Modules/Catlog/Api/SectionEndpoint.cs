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
    public async Task<ActionResult> AddSection(SectionDto sectionDto)
    {
        await _sectionService.AddSection(sectionDto);
        return Ok();
    }
}