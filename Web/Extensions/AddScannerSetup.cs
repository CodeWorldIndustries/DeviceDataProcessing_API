using Application.Device.Data;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Extensions
{

    /// <summary>
    /// The AddScannerSetup
    /// </summary>
    public static class AddScannerSetup
    {
        /// <summary>
        /// Adds the scanner extension.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void AddScannerExtension(this IServiceCollection services)
        {
            // You only need to reference one service per project for it to scan
            // all the assemblies the in the project and set up dependancy injection

            // Application project
            services.Scan(scan => scan.FromAssemblyOf<IDeviceDataService>().AddClasses().AsMatchingInterface());

            // Database project
            services.Scan(scan => scan.FromAssemblyOf<IRepositoryService>().AddClasses().AsMatchingInterface());
        }
    }
}
