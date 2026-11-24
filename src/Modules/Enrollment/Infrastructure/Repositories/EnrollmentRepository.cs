using ELearning.Enrollment.Domain;
using ELearning.Enrollment.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public class EnrollmentRepository
{
    private readonly EnrollmentDbContext _context;
    public EnrollmentRepository(EnrollmentDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Enrollment enrollment)
    {
        await _context.Enrollments.AddAsync(enrollment);
    }
    public async Task<bool> IsEnrolled(Guid studentId, Guid courseId)
    {
       return await _context.Enrollments.AnyAsync(e => e.StudentId == studentId && e.CourseId ==courseId && e.Status == EnrollmentStatus.Completed);
    }

    public async Task<IEnumerable<Enrollment>> GetAllCourseEnrolled(Guid studentId)
    {
        return await _context.Enrollments.Where(e => e.StudentId == studentId).ToListAsync();
    }
    public async Task<Enrollment?> getByIdAsync(Guid enrollmentId)
    {
        var enrollment =  await _context.Enrollments.FindAsync(enrollmentId);
        return enrollment;
    }
}