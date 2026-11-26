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
    private readonly InstructorService _instructorService1;
    public InstructorController(InstructorService instructorService)
    {
        _instructorService1 = instructorService;
    }

    [HttpGet("")]
    [Authorize(Policy = "InstructorOrAdmin")]
    public async Task<ActionResult> GetBindingEnrollments()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(!Guid.TryParse(userId,out var gId))
            return Unauthorized();

        var enrollments = await _instructorService1.GetBindingAllEnrollments(gId);
        return Ok(enrollments);
    }

    [HttpGet("/Confirm/{id}")]
    public async Task<ActionResult> EnrollmentConfirm(EnrollmentConfrimDto enrollmentConfrimDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(!Guid.TryParse(userId,out var gId))
            return Unauthorized();
        await _instructorService1.EnrollmentConfirm(enrollmentConfrimDto,gId);
        return Ok();
    }
}