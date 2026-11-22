using ELearning.Catalog.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class Lessoncontroller : ControllerBase
{
    private readonly LessonService _lessonService;
    public Lessoncontroller(LessonService lessonService)
    {
        _lessonService = lessonService;
    }
    [HttpPost("")]
    public async Task<ActionResult<LessonDto>> AddLesson(LessonDto lessonDto)
    {
        var lesson = await _lessonService.Add(lessonDto);
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
    public async Task<IActionResult> UploadVideo(
        Guid id,
        IFormFile video,
        CancellationToken cancellationToken)
    {
        await _lessonService.UploadVideoAsync(
            id,
            video,
            cancellationToken
        );

        return Ok(new
        {
            Message = "Video uploaded successfully"
        });
    }
}