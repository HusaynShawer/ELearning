using System.Security.Claims;
using ELearning.Catalog.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class Lessoncontroller : ControllerBase
{
    private readonly LessonService _lessonService;
    public Lessoncontroller(LessonService lessonService)
    {
        _lessonService = lessonService;
    }
    
    [HttpPost("")]
    [Authorize(Policy = "InstructorOrAdmin")]
    public async Task<ActionResult<LessonDto>> AddLesson(LessonDto lessonDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(!Guid.TryParse(userId,out var ownerId))
            return Forbid();

        var lesson = await _lessonService.Add(lessonDto,ownerId);
        return Ok(lesson);
    }
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<LessonDto>>> GetAll(Guid sectionId)
    {
        var lessons = await _lessonService.GetAll(sectionId);
        return Ok(lessons);
    }
    [HttpPost("{id}/video")]
    [Consumes("multipart/form-data")]
    [Authorize(Policy = "InstructorOrAdmin")]

    public async Task<IActionResult> UploadVideo(
        Guid id,
        IFormFile video,
        CancellationToken cancellationToken)
    {

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(!Guid.TryParse(userId,out var ownerId))
            return Forbid();

        await _lessonService.UploadVideoAsync(
            id,
            ownerId,
            video,
            cancellationToken
        );

        return Ok(new
        {
            Message = "Video uploaded successfully"
        });
    }
}