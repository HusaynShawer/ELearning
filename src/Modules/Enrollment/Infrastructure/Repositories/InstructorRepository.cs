using ELearning.Enrollment.Domain;
using ELearning.Enrollment.Infrastructure;
using Microsoft.EntityFrameworkCore;

public class InstructorRepository
{
    private readonly EnrollmentDbContext _enrollmentDbContext;
    public InstructorRepository(EnrollmentDbContext enrollmentDbContext)
    {
        _enrollmentDbContext = enrollmentDbContext;
    }

    public async Task<IEnumerable<Enrollment>> GetBindingEnrolls(Guid courseId)
    {
        var enrollments =  await _enrollmentDbContext.Enrollments.Where(e =>e.CourseId ==courseId && e.Status == EnrollmentStatus.Active).ToListAsync();
        return enrollments;
    }

    public async Task EditEnrollStauts(Enrollment enrollment)
    {
        _enrollmentDbContext.Enrollments.Update(enrollment);
        await _enrollmentDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Enrollment>> GetBindingAllEnrolls(Guid courseId)
    {
        var enrollments =  await _enrollmentDbContext.Enrollments.Where(e =>e.CourseId ==courseId && e.Status == EnrollmentStatus.Active).ToListAsync();
        return enrollments;
    }
}