using ELearning.Catalog.Infrastructure;
using ELearning.Enrollment.Application;
using ELearning.Enrollment.Domain;
using ELearning.Enrollment.Infrastructure;

public class EnrollmentService
{
    private readonly EnrollmentRepository _enrollmentRepository;
    private readonly LessonService _lessonService;
    private readonly EnrollmentUnitOfWork _uow;
    public EnrollmentService(EnrollmentRepository enrollmentRepository,
                            LessonService lessonService,
                            EnrollmentUnitOfWork uow)
    {
        _enrollmentRepository = enrollmentRepository;
        _lessonService = lessonService;
        _uow = uow;
    }

    public async Task AddAsync(EnrollmentDto enrollmentDto, Guid studentId)
    {
        var lessons = await _lessonService.GetByCourseId(enrollmentDto.CourseId);
        var enrollment = new Enrollment
        {
            StudentId = studentId,
            CourseId = enrollmentDto.CourseId,
            Status = EnrollmentStatus.Active,
            EnrolledAtUtc = DateTime.UtcNow,
            LessonProgresses = lessons.Select(l => new LessonProgress
            {
                LessonId = l.Id,
                IsCompleted =false,
                LastPositionSeconds = 0,
                CompletedAtUtc = null
            }).ToList(),
            
        };

        await _enrollmentRepository.AddAsync(enrollment);
        await _uow.SaveAsync();

    }
    public async Task<bool> IsEnrolled(Guid studentId, Guid courseId)
    {
       return await _enrollmentRepository.IsEnrolled(studentId,courseId);
    }

    public async Task<IEnumerable<Guid>> GetCoursesEnrolledIn(Guid studenId)
    {
        var enrolls = await _enrollmentRepository.GetAllCourseEnrolled(studenId);
        var coursesIds = new List<Guid>();
        foreach(var enroll in enrolls)
        {
            if(enroll is null)
                continue;
            coursesIds.Add(enroll.CourseId);
        }

        return coursesIds;
    }
}