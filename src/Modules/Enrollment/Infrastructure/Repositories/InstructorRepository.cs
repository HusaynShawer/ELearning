using ELearning.Enrollment.Domain;
using EnrollmentEntity = ELearning.Enrollment.Domain.Enrollment;
using Microsoft.EntityFrameworkCore;

namespace ELearning.Enrollment.Infrastructure;

public class InstructorRepository
{
    private readonly EnrollmentDbContext _enrollmentDbContext;

    public InstructorRepository(EnrollmentDbContext enrollmentDbContext)
    {
        _enrollmentDbContext = enrollmentDbContext;
    }

    public async Task<IEnumerable<EnrollmentEntity>> GetBindingEnrollmentsAsync(Guid courseId)
    {
        var enrollments = await _enrollmentDbContext.Enrollments.Where(e => e.CourseId == courseId && e.Status == EnrollmentStatus.Active).ToListAsync();
        return enrollments;
    }

    public async Task UpdateEnrollmentStatusAsync(EnrollmentEntity enrollment)
    {
        _enrollmentDbContext.Enrollments.Update(enrollment);
        await _enrollmentDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<EnrollmentEntity>> GetAllBindingEnrollmentsAsync(Guid courseId)
    {
        var enrollments = await _enrollmentDbContext.Enrollments.Where(e => e.CourseId == courseId && e.Status == EnrollmentStatus.Active).ToListAsync();
        return enrollments;
    }
}
