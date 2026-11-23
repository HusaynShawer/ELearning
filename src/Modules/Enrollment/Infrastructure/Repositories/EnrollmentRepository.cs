using ELearning.Enrollment.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

public class EnrollmentRepository
{
    private readonly EnrollmentDbContext _context;
    public EnrollmentRepository(EnrollmentDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync()
    {
        
    }
    public async Task IsEnrolled()
    {
        
    }
}