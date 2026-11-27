using ELearning.Catalog.Infrastructure;
using EnrollmentEntity = ELearning.Enrollment.Domain.Enrollment;
using ELearning.Enrollment.Infrastructure;
using ELearning.SharedKernel;
using ELearning.SharedKernel.Application.Exceptions;

namespace ELearning.Enrollment.Application;

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

    public async Task<IEnumerable<EnrollmentResponseDto>> GetBindingEnrollmentsAsync(Guid courseId, Guid instructorId)
    {
        var isOwner = await _helper.IsOwner(courseId, instructorId);
        if (isOwner)
        {
            var enrollments = await _instructorRepository.GetBindingEnrollmentsAsync(courseId);
            if (enrollments.Any() || enrollments is not null)
            {
                return enrollments.Select(e => new EnrollmentResponseDto
                {
                    Id = e.Id,
                    CourseId = e.CourseId,
                    Status = e.Status
                });
            }
            throw new NotFoundException("No enrollments found for this course.");
        }
        throw new ForbiddenException("Only the course owner can view enrollment requests.");
    }

    public async Task<IEnumerable<EnrollmentResponseDto>> GetBindingAllEnrollmentsAsync(Guid instructorId)
    {
        var courses = await _courseRepository.GetCoursesByInstructorAsync(instructorId);
        var coursesIds = new List<Guid>();

        if (courses is null || !courses.Any())
        {
            throw new NotFoundException("No courses found for this instructor.");
        }

        foreach (var item in courses)
        {
            if (courses is not null)
                coursesIds.Add(item.Id);
            else
                continue;
        }

        var instructorEnrollments = new List<EnrollmentEntity>();
        foreach (Guid Id in coursesIds)
        {
            var allEnrollment = await _enrollmentRepository.GetByCourseIdAsync(Id);
            if (allEnrollment is null || !allEnrollment.Any())
            {
                throw new NotFoundException("No enrollments found for this instructor.");
            }
            foreach (var item in allEnrollment)
            {
                if (item is null)
                    continue;
                instructorEnrollments.Add(item);
            }
        }
        return instructorEnrollments.Select(ie => new EnrollmentResponseDto
        {
            Id = ie.Id,
            CourseId = ie.CourseId,
            Status = ie.Status
        });
    }

    public async Task ConfirmEnrollmentAsync(EnrollmentConfirmDto enrollmentConfirmDto, Guid ownerId)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentConfirmDto.Id);
        if (enrollment is null)
            throw new NotFoundException("Enrollment not found.");

        var isOwner = await _helper.IsOwner(enrollment.CourseId, ownerId);
        if (!isOwner)
            throw new ForbiddenException("You do not have permission to confirm this enrollment.");

        if (enrollment is null)
            throw new NotFoundException("Enrollment not found.");

        enrollment.Status = enrollmentConfirmDto.Status;
        await _instructorRepository.UpdateEnrollmentStatusAsync(enrollment);
    }
}
