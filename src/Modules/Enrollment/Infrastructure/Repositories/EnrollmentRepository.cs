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
       return await _context.Enrollments.AnyAsync(e => e.StudentId == studentId && e.CourseId ==courseId);
    }
}