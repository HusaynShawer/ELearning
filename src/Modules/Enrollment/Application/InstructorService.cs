using ELearning.Enrollment.Application;
using ELearning.Enrollment.Domain;
using ELearning.SharedKernel;
public class InstructorService
{
    private readonly InstructorRepository _instructorRepository;
    private readonly EnrollmentRepository _enrollmentRepository;

    private readonly Helper _helper;

    public InstructorService(InstructorRepository instructorRepository, Helper helper,
                            EnrollmentRepository enrollmentRepository)
    {
        _helper = helper;
        _instructorRepository = instructorRepository;
        _enrollmentRepository = enrollmentRepository;
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

    public async Task EnrollmentConfirm(EnrollmentConfrimDto enrollmentConfrimDto)
    {
        var enrollment = await _enrollmentRepository.getByIdAsync(enrollmentConfrimDto.Id);
        if(enrollment is null)
            throw new Exception("no enrollment");
        enrollment.Status = enrollmentConfrimDto.Status;
        await _instructorRepository.EditEnrollStauts(enrollment);
    }
}