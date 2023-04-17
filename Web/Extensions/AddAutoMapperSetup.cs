using Application.Concentric;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Configuration
{
    /// <summary>
    /// The AddAutoMapperSetup
    /// </summary>
    public static class AddAutoMapperSetup
    {
        /// <summary>
        /// Adds the automatic mapper extension.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void AddAutoMapperExtension(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DeviceMapper());
            });
            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
