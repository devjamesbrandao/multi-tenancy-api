using MultiTenancy.Identity.Data;

namespace MultiTenancy.Api.Extensions
{
    public static class IdentityMigrations
    {
        public static async Task ApplyMigrationsIdentity(IApplicationBuilder serviceScope)
        {
            var services = serviceScope.ApplicationServices.CreateScope().ServiceProvider;

            using var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<IdentityDataContext>();

            await DbHealthChecker.TestConnection(context);

            await context.Database.EnsureCreatedAsync();
        }
    }
}