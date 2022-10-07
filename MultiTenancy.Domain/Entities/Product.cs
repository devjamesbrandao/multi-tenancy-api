using MultiTenancy.Domain.Entities.Shared;

namespace MultiTenancy.Domain.Entities;

public class Product : Entity
{
    public string Code { get; private set; }
    public int CategoryId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; set; }
    public DateTime RegistrationDate { get; private set; }
    
    public Category Category { get; private set; }

    public Product(int id, string code, int categoryId, string name, string description, decimal price)
    {
        Id = id;
        Code = code;
        CategoryId = categoryId;
        Name = name;
        Description = description;
        Price = price;
        RegistrationDate = DateTime.Now;
    }

    public Product(string code, int categoryId, string name, string description, decimal price)
        : this(default, code, categoryId, name, description, price) { }
            
}