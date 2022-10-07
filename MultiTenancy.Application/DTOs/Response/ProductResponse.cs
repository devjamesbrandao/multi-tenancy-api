using MultiTenancy.Domain.Entities;

namespace MultiTenancy.Application.DTOs.Response;

public class ProductResponse
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public DateTime DataRegister { get; set; }
    public CategoryResponse Category { get; set; }
    
    public ProductResponse(int id, string code, string name, string description, decimal price, DateTime dataRegister, CategoryResponse category)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Price = price;
        DataRegister = dataRegister;
        Category = category;
    }

    public static ProductResponse ConvertToResponse(Product product)
    {
        return new ProductResponse
        (
            product.Id,
            product.Code,
            product.Name,
            product.Description,
            product.Price,
            product.RegistrationDate,
            new CategoryResponse(product.Category.Id, product.Category.Name)
        );
    }
}