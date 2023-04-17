using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Extensions
{
    public static class CorsExtension
    {
        /// <summary>
        /// Adds the cors extension.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void AddCorsExtension(this IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed((host) => true)
                    .AllowCredentials();
                }));
        }

        /// <summary>
        /// Uses the cors extension.
        /// </summary>
        /// <param name="app">The application.</param>
        public static void UseCorsExtension(this IApplicationBuilder app)
        {
            app.UseCors("CorsPolicy");
        }
    }
}
