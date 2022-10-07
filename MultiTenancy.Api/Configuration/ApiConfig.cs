namespace MultiTenancy.Api.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfig(this IServiceCollection services)
        {   
            services.AddCors();

            services.AddControllers();

            services.AddMvc();
        }

        public static void UseApiConfig(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            
            app.UseDeveloperExceptionPage();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors(builder => builder
                .SetIsOriginAllowed(orign => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
            );

            app.UseEndpoints(endpoints =>{ endpoints.MapControllers(); });
        }
    }
}