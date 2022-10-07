using MultiTenancy.Domain.Entities;

namespace MultiTenancy.Application.DTOs.Request;

public class InsertProductRequest
{
    public string Code { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    
    public InsertProductRequest(string code, int categoryId, string name, string description, decimal price)
    {
        Code = code;
        CategoryId = categoryId;
        Name = name;
        Description = description;
        Price = price;
    }

    public static Product ConvertToEntity(InsertProductRequest productRequest)
    {
        return new Product
        (
            productRequest.Code,
            productRequest.CategoryId,
            productRequest.Name,
            productRequest.Description,
            productRequest.Price
        );
    }
}