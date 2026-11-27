using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using ELearning.Enrollment.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InstructorController : ControllerBase
{
    private readonly InstructorService _instructorService;
    public InstructorController(InstructorService instructorService)
    {
        _instructorService = instructorService;
    }

    [HttpGet("")]
    [Authorize(Policy = "InstructorOrAdmin")]
    public async Task<ActionResult> GetBindingEnrollments()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(!Guid.TryParse(userId,out var gId))
            return Unauthorized();

        var enrollments = await _instructorService.GetBindingAllEnrollmentsAsync(gId);
        return Ok(enrollments);
    }

    [HttpGet("/Confirm/{id}")]
    public async Task<ActionResult> EnrollmentConfirm(EnrollmentConfirmDto enrollmentConfirmDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(!Guid.TryParse(userId,out var gId))
            return Unauthorized();
        await _instructorService.ConfirmEnrollmentAsync(enrollmentConfirmDto,gId);
        return Ok();
    }
}