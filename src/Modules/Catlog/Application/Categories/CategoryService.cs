using ELearning.Catalog.Application.Dtos;
using ELearning.Catalog.Domain;
using ELearning.Catalog.Infrastructure;

namespace ELearning.Catalog.Application.Service;

public class CategoryService
{
    private readonly CategoryRepository _categoryRepository;

    public CategoryService(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllCategoriesAsync();
        return categories.Select(cat => new CategoryResponseDto
        {
            Id = cat.Id,
            Name = cat.Name,
            Slug = cat.Slug
        });
    }

    public async Task CreateCategoryAsync(CategoryDto cat)
    {
        var category = new Category
        {
            Name = cat.Name,
            Slug = cat.Slug
        };
        await _categoryRepository.AddCategoryAsync(category);
    }
}
