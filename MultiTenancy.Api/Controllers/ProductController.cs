using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenancy.Application.DTOs.Request;
using MultiTenancy.Application.DTOs.Response;
using MultiTenancy.Domain.Interfaces.Services;
using MultiTenancy.Identity;

namespace MultiTenancy.Api.Controllers;

[Authorize(Roles = Roles.Admin)]
[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private IProductService _productService;

    public ProductController(IProductService productService) => _productService = productService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAll()
    {
        var products = await _productService.GetAllAsync();

        var productsResponse = products.Select(product => ProductResponse.ConvertToResponse(product));

        return Ok(productsResponse);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponse>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product is null) return NotFound();
            
        var productResponse = ProductResponse.ConvertToResponse(product);

        return Ok(productResponse);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Insert(InsertProductRequest productRequest)
    {
        var product = InsertProductRequest.ConvertToEntity(productRequest);

        var id = (int)await _productService.AddAsync(product);

        return CreatedAtAction(nameof(GetById), new { id = id }, id);
    }

    [HttpPut]
    public async Task<ActionResult> Update(UpdateProductRequest productRequest)
    {
        var product = UpdateProductRequest.ConvertToEntity(productRequest);

        await _productService.UpdateAsync(product);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _productService.RemoveByIdAsync(id);
        
        return Ok();
    }
}