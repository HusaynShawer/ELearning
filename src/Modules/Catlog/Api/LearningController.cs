using System.Security.Claims;
using ELearning.Catalog.Application;
using ELearning.Catalog.Application.Dtos;
using ELearning.Enrollment.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LearningController : ControllerBase
{
    private readonly LearningService _learningService;
    private readonly EnrollmentService _enrollmentService;

    public LearningController(LearningService learningService,
                                EnrollmentService enrollmentService)
    {
        _learningService = learningService;
        _enrollmentService = enrollmentService;
    }

    [HttpGet("{courseId}")]
    public async Task<ActionResult<CourseResponseDto>> GetById(Guid courseId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var studentId))
            return Unauthorized();

        var isEnrolled = await _enrollmentService.IsEnrolledAsync(studentId, courseId);
        if (!isEnrolled)
            return Forbid();

        var course = await _learningService.GetByIdAsync(courseId);
        return Ok(course);
    }
}
