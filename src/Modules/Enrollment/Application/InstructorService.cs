using ELearning.Catalog.Domain;
using ELearning.Enrollment.Application;
using ELearning.Enrollment.Domain;
using ELearning.SharedKernel;
public class InstructorService
{
    private readonly InstructorRepository _instructorRepository;
    private readonly EnrollmentRepository _enrollmentRepository;
    private readonly CourseRepository _courseRepository;


    private readonly Helper _helper;

    public InstructorService(InstructorRepository instructorRepository, Helper helper,
                            CourseRepository courseRepository,
                            EnrollmentRepository enrollmentRepository)
    {
        _helper = helper;
        _instructorRepository = instructorRepository;
        _enrollmentRepository = enrollmentRepository;
        _courseRepository = courseRepository;
    }

    public async Task<IEnumerable<EnrollmentReponseDto>> GetBindigEnrollments(Guid courseId, Guid instructorId)
    {
        var isOwner = await _helper.IsOwner(courseId,instructorId);
        if (isOwner)
        {
            var enrollments = await _instructorRepository.GetBindingEnrolls(courseId);
            if (enrollments.Any() || enrollments is not null)
            {
                return enrollments.Select(e => new EnrollmentReponseDto
                {
                    Id = e.Id,
                    CourseId = e.CourseId,
                    Status = e.Status
                });
            }
            throw new Exception("only owner can see binding enrollments");

        }
        throw new Exception("only owner can see binding enrollments");
    }


    public async Task<IEnumerable<EnrollmentReponseDto>> GetBindingAllEnrollments(Guid instructorId)
    {
        var courses = await _courseRepository.GetCoursesByInstructor(instructorId);
        var coursesIds = new List<Guid>();
        
        if(courses is null || !courses.Any())
        {
            throw new Exception("no course for you");
        }

        foreach (var item in courses)
        {
            if (courses is not null)
                coursesIds.Add(item.Id);
            else
                continue;
        }

        var instructorEnrollments = new List<Enrollment>();
        foreach(Guid Id in coursesIds)
        {
            var allEnrollment = await _enrollmentRepository.GetByCourseIdAsync(Id);
            if (allEnrollment is null || !allEnrollment.Any())
            {
                throw new Exception("no enrollments for you");
            }
            foreach(var item in allEnrollment)
            {
                if(item is null)
                    continue;
                instructorEnrollments.Add(item);
                    
            }   
        }
        return instructorEnrollments.Select(ie =>new EnrollmentReponseDto
        {
            Id = ie.Id,
            CourseId = ie.CourseId,
            Status = ie.Status
        });

    }

    public async Task EnrollmentConfirm(EnrollmentConfrimDto enrollmentConfrimDto, Guid ownerId)
    {
        var enrollment = await _enrollmentRepository.getByIdAsync(enrollmentConfrimDto.Id);
        if (enrollment is null)
            throw new Exception("no enrollent with this id");
        var isOwner = await _helper.IsOwner(enrollment.CourseId,ownerId);
        if (!isOwner)
            throw new Exception("you have no right to confirm");
        
        if(enrollment is null)
            throw new Exception("no enrollment");
        enrollment.Status = enrollmentConfrimDto.Status;
        await _instructorRepository.EditEnrollStauts(enrollment);
    }
}