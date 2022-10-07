
using MultiTenancy.Api.Configuration;
using MultiTenancy.Api.Extensions;

namespace MultiTenancy.Api;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApiConfig();

        services.AddSwaggerConfig();

        services.AddAuthentication(Configuration);

        services.RegisterServices(Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        IdentityMigrations.ApplyMigrationsIdentity(app).Wait();
        
        app.UseSwaggerUIConfig();

        app.UseApiConfig(env);
    }
}