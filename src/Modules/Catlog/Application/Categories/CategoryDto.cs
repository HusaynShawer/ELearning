
using System.ComponentModel.DataAnnotations;
namespace ELearning.Catalog.Application.Dtos;

public class CategoryDto
{
    [Required]
    public string Name {get; set;}
    public string Slug {get;set;}
}

public class CategoryResponseDto
{
    public Guid Id {get; set;}
    [Required]
    public string Name {get; set;}
    public string Slug {get;set;}
}
