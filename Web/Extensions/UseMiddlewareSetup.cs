using Microsoft.AspNetCore.Builder;
using Web.Middleware;

namespace Web.Extensions
{
    public static class UseMiddlewareSetup
    {
        /// <summary>
        /// Uses the middleware extension.
        /// </summary>
        /// <param name="app">The application.</param>
        public static void UseMiddlewareExtension(this IApplicationBuilder app)
        {
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
        }
    }
}
