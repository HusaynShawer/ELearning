namespace ELearning.Enrollment.Infrastructure;

public class EnrollmentUnitOfWork
{
    private readonly EnrollmentDbContext _context;
    public EnrollmentUnitOfWork(EnrollmentDbContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}