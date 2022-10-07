using MultiTenancy.Domain.Entities;

namespace MultiTenancy.Application.DTOs.Request;

public class UpdateProductRequest
{
    public int Id { get; set; }
    public string Code { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    
    public UpdateProductRequest(string code, int categoryId, string name, string description, decimal price)
    {
        Code = code;
        CategoryId = categoryId;
        Name = name;
        Description = description;
        Price = price;
    }

    public static Product ConvertToEntity(UpdateProductRequest productRequest)
    {
        return new Product
        (
            productRequest.Id,
            productRequest.Code,
            productRequest.CategoryId,
            productRequest.Name,
            productRequest.Description,
            productRequest.Price
        );
    }
}