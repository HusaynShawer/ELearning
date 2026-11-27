using ELearning.Catalog.Application.Dtos;
using ELearning.Catalog.Domain;
using ELearning.Catalog.Infrastructure;
using ELearning.Enrollment.Application;
using ELearning.SharedKernel.Application.Exceptions;

namespace ELearning.Catalog.Application;

public class LearningService
{
    private readonly CourseRepository _courseRepo;
    private readonly EnrollmentService _enrollmentService;

    private readonly UnitOfWork _uow;

    public LearningService(CourseRepository courseRepository,
                            EnrollmentService enrollmentService,
                            UnitOfWork uow)
    {
        _courseRepo = courseRepository;
        _enrollmentService = enrollmentService;
        _uow = uow;
    }

    public async Task<CourseResponseDto> GetByIdAsync(Guid courseId)
    {
        var course = await _courseRepo.GetCourseByIdAsync(courseId);
        if (course == null)
            throw new NotFoundException("Course not found.");

        return new CourseResponseDto
        {
            CourseId = course.Id,
            Title = course.Title,
            Description = course.Description,
            ThumbnailUrl = course.ThumbnailUrl,
            Price = course.Price,
            Level = course.Level,
            CategoryId = course.CategoryId,
            InstructorId = course.InstructorId,

            Sections = course.Sections.Select(s => new SectionWithCourseDto
            {
                Title = s.Title,
                Order = s.Order,
                Lessons = s.Lessons.Select(l => new LessonWithSectionDto
                {
                    Title = l.Title,
                    Order = l.Order,
                    Type = l.Type,
                    ContentUrl = l.ContentUrl,
                    DurationSeconds = l.DurationSeconds,
                }).ToList()
            }).ToList()
        };
    }

    public async Task<IEnumerable<CourseResponseDto>> GetAllCoursesEnrolledInAsync(Guid studentId)
    {
        var coursesIds = await _enrollmentService.GetCoursesEnrolledInAsync(studentId);
        var courses = new List<Course>();
        foreach (var courseId in coursesIds)
        {
            var course = await _courseRepo.GetCourseByIdAsync(courseId);
            if (course is null)
                continue;
            courses.Add(course);
        }

        return courses.Select(course => new CourseResponseDto
        {
            CourseId = course.Id,
            Title = course.Title,
            Description = course.Description,
            ThumbnailUrl = course.ThumbnailUrl,
            Price = course.Price,
            Level = course.Level,
            CategoryId = course.CategoryId,
            InstructorId = course.InstructorId,

            Sections = course.Sections.Select(s => new SectionWithCourseDto
            {
                Title = s.Title,
                Order = s.Order,
                Lessons = s.Lessons.Select(l => new LessonWithSectionDto
                {
                    Title = l.Title,
                    Order = l.Order,
                    ContentUrl = l.ContentUrl,
                    Type = l.Type,
                    DurationSeconds = l.DurationSeconds,
                }).ToList()
            }).ToList()
        });
    }
}
