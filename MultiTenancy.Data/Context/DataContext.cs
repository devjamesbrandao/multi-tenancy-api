using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MultiTenancy.Data.DTO;
using MultiTenancy.Data.Extensions;
using MultiTenancy.Domain.Entities;

namespace MultiTenancy.Data.Context;

public class DataContext : DbContext
{
    private readonly IHttpContextAccessor _acessor;

    private Tenants _tenant { get; set; }

    public bool IsAuthenticated() => _acessor.HttpContext?.User?.Identity.IsAuthenticated ?? false;

    public DataContext(
        IOptions<List<Tenants>> tenants,
        IHttpContextAccessor accessor,
        DbContextOptions<DataContext> options
    ) : base(options)
    { 
        _acessor = accessor;

        if(IsAuthenticated())
        {
            var idUser = Guid.Parse(_acessor.HttpContext.User.GetUserId());

            _tenant = tenants.Value.FirstOrDefault(x => x.UserId == idUser);
            
            if(_tenant is null) throw new Exception("Config user tenant Not Found");
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(IsAuthenticated())
        {
            if(_tenant.DbProvider.ToLower() == "sqlserver")
            {
                optionsBuilder
                    .UseSqlServer(connectionString: _tenant.ConnectionString)
                    .LogTo(Console.WriteLine, LogLevel.Information);
            }
            else
            {
                optionsBuilder
                    .UseNpgsql(connectionString: _tenant.ConnectionString)
                    .LogTo(Console.WriteLine, LogLevel.Information);
            }
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<string>()
            .AreUnicode(false)
            .HaveMaxLength(500);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var providerName = this.Database.ProviderName.ToLower();

        if(providerName.Contains("sqlserver", StringComparison.InvariantCultureIgnoreCase))
            ConfigModelCreatingSqlServer(modelBuilder);
        else if(providerName.Contains("postgres", StringComparison.InvariantCultureIgnoreCase))
            ConfigModelCreatingPostGres(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private void ConfigModelCreatingSqlServer(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MultiTenancy.Data.Mappings.SqlServer.CategoryMap());
        modelBuilder.ApplyConfiguration(new MultiTenancy.Data.Mappings.SqlServer.ProductMap());
    }

    private void ConfigModelCreatingPostGres(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MultiTenancy.Data.Mappings.PostGres.CategoryMap());
        modelBuilder.ApplyConfiguration(new MultiTenancy.Data.Mappings.PostGres.ProductMap());
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categorys { get; set; }
}