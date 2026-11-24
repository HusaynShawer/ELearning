using System.Security.Claims;
using ELearning.Catalog.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LearningController :ControllerBase
{
    private readonly LeariningService _leariningService;
    private readonly EnrollmentService _enrollmentService;

    public LearningController(LeariningService leariningService,
                                EnrollmentService enrollmentService)
    {
        _leariningService = leariningService;
        _enrollmentService = enrollmentService;
    }

    [HttpGet("{courseId}")]
    public async Task<ActionResult<CourseResponeDto>> GetById(Guid courseId)
    {
        var userId= User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var studentId))
            return Unauthorized();

        var isEnrolled = await _enrollmentService.IsEnrolled(studentId, courseId);
        if (!isEnrolled)
            return Forbid();

        var course = await _leariningService.GetByIdAsync(courseId);
        return Ok(course);

    }
}