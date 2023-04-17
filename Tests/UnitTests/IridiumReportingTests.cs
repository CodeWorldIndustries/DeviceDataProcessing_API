using Microsoft.Extensions.Configuration;
using Tests.UnitTests.common;
using Xunit;

namespace Tests.UnitTests
{
    /// <summary>
    /// Iridium unit tests
    /// </summary>
    public class IridiumReportingTests : IClassFixture<AppSettingsFixture>
    {
        private readonly string _secretKey;
        private readonly string _serviceProviderAccountNumber;
        private readonly string _username;

        private static readonly HttpClient httpClient = new HttpClient();
        private readonly string _endpoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="IridiumTests"/> class.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        public IridiumReportingTests(AppSettingsFixture fixture)
        {
            _endpoint = fixture.Configuration.GetValue<string>("Iridium:Endpoints:ReportWebServices");
            _secretKey = fixture.Configuration.GetValue<string>("Iridium:SecretKey");
            _serviceProviderAccountNumber = fixture.Configuration.GetValue<string>("Iridium:ServiceProviderAccountNumber");
            _username = fixture.Configuration.GetValue<string>("Iridium:Username");
        }


    }
}