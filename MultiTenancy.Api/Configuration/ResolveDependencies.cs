using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MultiTenancy.Application.Interfaces.Services;
using MultiTenancy.Data.DTO;
using MultiTenancy.Data.Repositories;
using MultiTenancy.Domain.Interfaces.Repositories;
using MultiTenancy.Domain.Interfaces.Services;
using MultiTenancy.Domain.Services;
using MultiTenancy.Identity.Data;
using MultiTenancy.Identity.Services;

namespace MultiTenancy.Api.Configuration
{
    public static class ResolveDependencies
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<List<Tenants>>(configuration.GetSection(nameof(Tenants)));

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAndMigrateTenantDatabasesConfig();
            
            services.AddDbContext<IdentityDataContext>(options =>
                options.UseSqlServer(
                    connectionString: configuration.GetConnectionString("IdentityConnectionString"),
                    sqlServerOptionsAction: c => c.MigrationsAssembly(typeof(IdentityDataContext).Assembly.FullName)
                )
                .LogTo(Console.WriteLine, LogLevel.Information)
            );
            
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityDataContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IIdentityService, IdentityService>(); 

            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddScoped<IProductRepository, ProductRepository>();
        }
    }
}