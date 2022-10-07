using MultiTenancy.Domain.Entities.Shared;

namespace MultiTenancy.Domain.Entities;

public class Category : Entity
{
    public string Name { get; set; }

    public ICollection<Product> Products { get; private set; }
    
    public Category(int id, string name)
    {
        Id = id;
        Name = name;
    }
}