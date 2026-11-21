
using ELearning.Catalog.Domain;
using ELearning.Catalog.Infrastructure;
using Microsoft.EntityFrameworkCore;

public class CategoryRepository
{
    private readonly CatalogDbContext _context;
    public CategoryRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task AddCategoryAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }
    public async Task<Category?> GetCategoryAsync(Guid categoryId)
    {
        return await _context.Categories.FindAsync(categoryId);
        
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        var categories = await _context.Categories.ToListAsync();
        return categories;
    }   
}