using ELearning.Catalog.Application.Dtos;
using ELearning.Catalog.Domain;
using ELearning.Catalog.Infrastructure;

public class CourseService
{
    private readonly CourseRepository _courseRepo;
    private readonly UnitOfWork _uow;
    public CourseService(CourseRepository courseRepository,
                            UnitOfWork uow)
    {
        _courseRepo = courseRepository;
        _uow = uow;
    }

    public async Task<CourseResponeDto> CreateAsync(CourseRequestDto dto)
    {
        var course = new Course
        {
            Title = dto.Title,
            Slug = dto.Slug,
            Description = dto.Description,
            ThumbnailUrl = dto.ThumbnailUrl,
            Price = dto.Price,
            Currency = "EGP",
            Level = dto.Level,
            Status = dto.Status,
            CategoryId = dto.CategoryId,
            InstructorId = dto.InstructorId, // <-- must set this

            Sections = dto.Sections.Select(s => new Section
            {
                Title = s.Title,
                Order = s.Order,
                Lessons = s.Lessons.Select(l => new Lesson
                {
                    Title = l.Title,
                    Order = l.Order,
                    Type = l.Type,
                    ContentUrl = l.ContentUrl,
                    DurationSeconds = l.DurationSeconds,
                }).ToList()
            }).ToList()
        };
        
        await _courseRepo.AddCourseAsync(course);
        await _uow.SaveAsync();
        return new CourseResponeDto
        {
            CourseId     = course.Id,
            Title        = course.Title,
            Description  = course.Description,
            ThumbnailUrl = course.ThumbnailUrl,
            Price        = course.Price,
            Level        = course.Level,
            CategoryId   = course.CategoryId,
            InstructorId = course.InstructorId,

            Sections = course.Sections.Select(s => new SectionWithCourseDto
            {
                Title = s.Title,
                Order = s.Order,
                Lessons = s.Lessons.Select(l => new LessonWithSectionDto
                {
                    Title           = l.Title,
                    Order           = l.Order,
                    Type            = l.Type,
                    ContentUrl      = l.ContentUrl,
                    DurationSeconds = l.DurationSeconds,
                }).ToList()
            }).ToList()
        };
    }

    public async Task<CourseResponeDto> GetByIdAsync(Guid courseId)
    {
        var course = await _courseRepo.GetCourseByIdAsync(courseId);
        if (course == null)
            throw new Exception("Course not found");

        return new CourseResponeDto
        {
            CourseId     = course.Id,
            Title        = course.Title,
            Description  = course.Description,
            ThumbnailUrl = course.ThumbnailUrl,
            Price        = course.Price,
            Level        = course.Level,
            CategoryId   = course.CategoryId,
            InstructorId = course.InstructorId,

            Sections = course.Sections.Select(s => new SectionWithCourseDto
            {
                Title = s.Title,
                Order = s.Order,
                Lessons = s.Lessons.Select(l => new LessonWithSectionDto
                {
                    Title           = l.Title,
                    Order           = l.Order,
                    Type            = l.Type,
                    ContentUrl      = l.ContentUrl,
                    DurationSeconds = l.DurationSeconds,
                }).ToList()
            }).ToList()
        };
    }

    internal async Task<IEnumerable<CourseResponeDto>> GetAllCourses()
    {
        var courses = await _courseRepo.GetAllCoursesAsync();
        return courses.Select(course => new CourseResponeDto
        {
            CourseId     = course.Id,
            Title        = course.Title,
            Description  = course.Description,
            ThumbnailUrl = course.ThumbnailUrl,
            Price        = course.Price,
            Level        = course.Level,
            CategoryId   = course.CategoryId,
            InstructorId = course.InstructorId,

            Sections = course.Sections.Select(s => new SectionWithCourseDto
            {
                Title = s.Title,
                Order = s.Order,
                Lessons = s.Lessons.Select(l => new LessonWithSectionDto
                {
                    Title           = l.Title,
                    Order           = l.Order,
                    Type            = l.Type,
                    ContentUrl      = l.ContentUrl,
                    DurationSeconds = l.DurationSeconds,
                }).ToList()
            }).ToList()
        });
    }
    }