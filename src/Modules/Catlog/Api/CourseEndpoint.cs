using Microsoft.AspNetCore.Mvc;
using ELearning.Catalog.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CourseController : ControllerBase
{
    private readonly CourseService _courseService;
    public CourseController(CourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpPost("")]
    public async Task<ActionResult<CourseResponeDto>> CreateCourse([FromBody] CourseRequestDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(!Guid.TryParse(userId,out var ownerId))
            return Forbid();
            
        var result = await _courseService.CreateAsync(dto, ownerId);
        return CreatedAtAction(nameof(GetCourseById), new { courseId = result.CourseId }, result);
    }

    [HttpGet("{courseId}")]
    public async Task<ActionResult<CourseResponeDto>> GetCourseById(Guid courseId)
    {
        var result = await _courseService.GetByIdAsync(courseId);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<CourseResponeDto>>> GetAllCourses()
    {
        var result = await _courseService.GetAllCourses();
        return Ok(result);
    }
}