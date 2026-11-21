using ELearning.Catalog.Application.Dtos;
using ELearning.Catalog.Domain;

namespace ELearning.Catalog.Application.Service;

public class categoryService
{
    private readonly CategoryRepository _categoryRepository;

    public categoryService(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync()
    {
        var categories =  await _categoryRepository.GetAllCategoriesAsync();
        return categories.Select(cat => new CategoryResponseDto
        {
            Id = cat.Id,
            Name = cat.Name,
            slug = cat.Slug
        });
    }

    public async Task CreateCategory(CategoryDto cat)
    {
        var category = new Category
        {
            Name = cat.Name,
            Slug = cat.slug
        };
        await _categoryRepository.AddCategoryAsync(category);
    }
}