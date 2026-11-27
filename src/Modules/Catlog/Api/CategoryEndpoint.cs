using ELearning.Catalog.Application.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ELearning.Catalog.Application.Dtos;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy ="AdminOnly")]
public class CategoryEndpoint :ControllerBase
{
    private readonly CategoryService _categoryService;
    public CategoryEndpoint(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost("")]
    public async Task<ActionResult> Create(CategoryDto cat)
    {
        await _categoryService.CreateCategoryAsync(cat);
        return Ok();
    }
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAlls()
    {
        var category = await _categoryService.GetAllCategoriesAsync();
        return Ok(category);
    }
}
