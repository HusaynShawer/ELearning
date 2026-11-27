using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ELearning.Enrollment.Application;
using System.Security.Claims;
using Microsoft.VisualBasic;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EnrollmentController :ControllerBase
{
    private readonly EnrollmentService _enrollmentService;
    public EnrollmentController(EnrollmentService enrollmentService)
    {
     _enrollmentService = enrollmentService;   
    }

    [Authorize]
    [HttpPost("")]
    public async Task<ActionResult<EnrollmentDto>> CreateEnrollment(EnrollmentDto enrollmentDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userId, out var studentId))
            return Unauthorized();

        await _enrollmentService.AddAsync(enrollmentDto, studentId);

        return Ok();
    }
    
    [Authorize]
    [HttpGet("")]
    public async Task<ActionResult> IsEnrolled(EnrollmentDto enrollmentDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var studentId))
            return Unauthorized(); 
        await _enrollmentService.IsEnrolledAsync(enrollmentDto.CourseId, studentId);
        return Ok();
    }
}