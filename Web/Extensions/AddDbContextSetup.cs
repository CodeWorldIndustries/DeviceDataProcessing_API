using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Extensions
{
    public static class AddDbContextSetup
    {
        /// <summary>
        /// Adds the database context extension.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void AddDbContextExtension(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var connection = configuration.GetSection("Database:ConnectionString").Value;
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));
        }
    }
}