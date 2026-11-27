using ELearning.Enrollment.Domain;
using EnrollmentEntity = ELearning.Enrollment.Domain.Enrollment;
using Microsoft.EntityFrameworkCore;

namespace ELearning.Enrollment.Infrastructure;

public class EnrollmentRepository
{
    private readonly EnrollmentDbContext _context;

    public EnrollmentRepository(EnrollmentDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(EnrollmentEntity enrollment)
    {
        await _context.Enrollments.AddAsync(enrollment);
    }

    public async Task<bool> IsEnrolledAsync(Guid studentId, Guid courseId)
    {
        return await _context.Enrollments.AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId && e.Status == EnrollmentStatus.Completed);
    }

    public async Task<IEnumerable<EnrollmentEntity>> GetAllCoursesEnrolledAsync(Guid studentId)
    {
        return await _context.Enrollments.Where(e => e.StudentId == studentId).ToListAsync();
    }

    public async Task<EnrollmentEntity?> GetByIdAsync(Guid enrollmentId)
    {
        var enrollment = await _context.Enrollments.FindAsync(enrollmentId);
        return enrollment;
    }

    public async Task<IEnumerable<EnrollmentEntity>> GetByCourseIdAsync(Guid courseId)
    {
        var enrollment = await _context.Enrollments.Where(e => e.CourseId == courseId).ToListAsync();
        return enrollment;
    }
}
