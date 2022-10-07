using MultiTenancy.Domain.Entities;
using MultiTenancy.Domain.Interfaces.Repositories;
using MultiTenancy.Domain.Interfaces.Services;
using MultiTenancy.Domain.Services.Shared;

namespace MultiTenancy.Domain.Services;

public class ProductService : ServiceBase<Product>, IProductService
{
    public ProductService(IProductRepository productRepository) : base(productRepository) { }
}