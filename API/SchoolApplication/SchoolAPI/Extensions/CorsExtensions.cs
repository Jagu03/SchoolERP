namespace SchoolAPI.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
        {
            var corsSettings = configuration.GetSection("CorsSettings");
            var allowedOrigins = corsSettings.GetValue<string>("AllowedOrigins")?.Split(',') ?? Array.Empty<string>();

            services.AddCors(options =>
            {
                options.AddPolicy("SchoolApiPolicy", builder =>
                {
                    if (allowedOrigins.Length > 0)
                    {
                        builder.WithOrigins(allowedOrigins);
                    }
                    else
                    {
                        // Default policy if not configured
                        builder.AllowAnyOrigin();
                    }

                    builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("Token-Expired");
                });
            });

            return services;
        }

        public static IApplicationBuilder UseCustomCors(this IApplicationBuilder app)
        {
            return app.UseCors("SchoolApiPolicy");
        }
    }
}
