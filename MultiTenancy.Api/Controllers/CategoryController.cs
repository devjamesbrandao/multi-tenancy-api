using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenancy.Application.DTOs.Response;
using MultiTenancy.Domain.Interfaces.Repositories;
namespace MultiTenancy.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private ICategoryRepository _categoryRepository;

    public CategoryController(ICategoryRepository categoryRepository) => _categoryRepository = categoryRepository;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetAll()
    {
        var categorys = await _categoryRepository.GetAllAsync();

        var categorysResponse = categorys.Select(category => CategoryResponse.ConvertToResponse(category));

        return Ok(categorysResponse);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryResponse>> GetById(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category is null) return NotFound();
            
        var categoryResponse = CategoryResponse.ConvertToResponse(category);

        return Ok(categoryResponse);
    }
}