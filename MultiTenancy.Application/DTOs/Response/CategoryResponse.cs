using MultiTenancy.Domain.Entities;

namespace MultiTenancy.Application.DTOs.Response;

public class CategoryResponse
{
    public int Id { get; set; }
    public string Name { get; set; }

    public CategoryResponse(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public static CategoryResponse ConvertToResponse(Category category)
    {
        return new CategoryResponse
        (
            category.Id,
            category.Name
        );
    }
}