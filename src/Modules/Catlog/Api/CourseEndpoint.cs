using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using ELearning.Catalog.Application;
using ELearning.Catalog.Application.Dtos;

[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{
    private readonly CourseService _courseService;
    public CourseController(CourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpPost("api/courses")]
    public async Task<ActionResult<CourseResponeDto>> CreateCourse([FromBody] CourseRequestDto dto)
    {
        var result = await _courseService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetCourseById), new { courseId = result.CourseId }, result);
    }

    [HttpGet("api/courses/{courseId}")]
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