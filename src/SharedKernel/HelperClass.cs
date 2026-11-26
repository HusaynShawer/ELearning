using ELearning.Catalog.Domain;
namespace ELearning.SharedKernel;
public class Helper
{
    private readonly CourseService _courseService;
    public Helper(CourseService courseService)
    {
        _courseService = courseService;
    }

    public async Task<bool> IsOwner(Guid courseId, Guid ownerId)
    {
        var course = await _courseService.GetByIdAsync(courseId);
        if(course is null || course.InstructorId != ownerId)
            return false;
        return true;
    }
}