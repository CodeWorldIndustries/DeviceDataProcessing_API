using Microsoft.AspNetCore.Builder;

namespace Web.Extensions
{
    public static class UseEndpointsSetup
    {
        /// <summary>
        /// Uses the endpoints extension.
        /// </summary>
        /// <param name="app">The application.</param>
        public static void UseEndpointsExtension(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
