using Microsoft.Extensions.Configuration;

namespace Tests.UnitTests.common
{
    public class AppSettingsFixture : IDisposable
    {
        public IConfiguration Configuration { get; private set; }

        public AppSettingsFixture()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public void Dispose()
        {
            // Clean up any resources here
        }
    }
}
