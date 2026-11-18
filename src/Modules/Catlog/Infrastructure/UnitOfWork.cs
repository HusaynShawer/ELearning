namespace ELearning.Catalog.Infrastructure;

public class UnitOfWork
{
    private readonly CatalogDbContext _context;
    public UnitOfWork(CatalogDbContext context)
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