using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;

namespace Web.Extensions
{
    public static class AddAuthSetup
    {
        /// <summary>
        /// Adds the authorization extension.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void AddAuthExtension(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"));
        }
    }
}
