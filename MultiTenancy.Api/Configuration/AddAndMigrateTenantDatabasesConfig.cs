using Microsoft.EntityFrameworkCore;
using MultiTenancy.Data.Context;
using MultiTenancy.Data.DTO;

namespace MultiTenancy.Api.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAndMigrateTenantDatabasesConfig(this IServiceCollection services)
        {
            var tenants = services.GetTenants(nameof(Tenants));

            foreach(var tenant in tenants)
            {
                switch(tenant.DbProvider.ToLower())
                {
                    case "sqlserver":
                        services.AddDbContext<DataContext>(c => c.UseSqlServer(tenant.ConnectionString));
                    break;

                    case "postgres":
                        services.AddDbContext<DataContext>(c => c.UseNpgsql(tenant.ConnectionString));
                    break;

                    default:
                        throw new Exception("DbProvider Not Valid");
                };

                using var scope = services.BuildServiceProvider().CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

                dbContext.Database.SetConnectionString(tenant.ConnectionString);

                dbContext.Database.EnsureCreated();

                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DataContext>));

                if (descriptor != null) services.Remove(descriptor);
            }

            services.AddDbContext<DataContext>();
        }

        public static List<Tenants> GetTenants(this IServiceCollection services, string sectionName)
        {
            using var serviceProvider = services.BuildServiceProvider();

            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var section = configuration.GetSection(sectionName);

            var options = new List<Tenants>();

            section.Bind(options);
            
            return options;
        }
    }
}