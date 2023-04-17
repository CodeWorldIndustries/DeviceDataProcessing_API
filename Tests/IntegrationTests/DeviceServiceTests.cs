using Application.Device.Data;
using Application.DeviceData.services;
using AutoMapper;
using CrossCutting.Common.Response;
using Domain.Devices.Domain;
using Domain.Devices.Models.Company;
using Domain.Devices.Models.Partners;
using IoTDeviceService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace IntegrationTests
{
    public class IoTControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly IMapper _mapper;

        public IoTControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PartnerData, IoTData>()
                 .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.PartnerId))
                 .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.PartnerName))
                 .ForMember(dest => dest.DeviceId, opt => opt.MapFrom(src => src.Trackers.Select(d => d.Id).FirstOrDefault()))
                 .ForMember(dest => dest.DeviceName, opt => opt.MapFrom(src => src.Trackers.Select(d => d.Model).FirstOrDefault()))
                 .ForMember(dest => dest.FirstReadingDtm, opt => opt.MapFrom(src => src.Trackers.SelectMany(t => t.Sensors).SelectMany(s => s.Crumbs).Min(c => c.CreatedDtm)))
                 .ForMember(dest => dest.LastReadingDtm, opt => opt.MapFrom(src => src.Trackers.SelectMany(t => t.Sensors).SelectMany(s => s.Crumbs).Max(c => c.CreatedDtm)))
                 .ForMember(dest => dest.TemperatureCount, opt => opt.MapFrom(src => src.Trackers.SelectMany(t => t.Sensors).Count(s => s.Name == "Temperature")))
                 .ForMember(dest => dest.AverageTemperature, opt => opt.MapFrom(src => src.Trackers.SelectMany(t => t.Sensors).Where(s => s.Name == "Temperature").Average(s => s.Crumbs.Average(c => c.Value))))
                 .ForMember(dest => dest.HumidityCount, opt => opt.MapFrom(src => src.Trackers.SelectMany(t => t.Sensors).Count(s => s.Name == "Humidty")))
                 .ForMember(dest => dest.AverageHumidity, opt => opt.MapFrom(src => src.Trackers.SelectMany(t => t.Sensors).Where(s => s.Name == "Humidty").Average(s => s.Crumbs.Average(c => c.Value))));

                cfg.CreateMap<CompanyData, IoTData>()
                    .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                    .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company))
                    .ForMember(dest => dest.DeviceId, opt => opt.MapFrom(src => src.Devices.FirstOrDefault().DeviceID))
                    .ForMember(dest => dest.DeviceName, opt => opt.MapFrom(src => src.Devices.FirstOrDefault().Name))
                    .ForMember(dest => dest.FirstReadingDtm, opt => opt.MapFrom(src => src.Devices.SelectMany(d => d.SensorData).Min(sd => DateTime.Parse(sd.DateTime))))
                    .ForMember(dest => dest.LastReadingDtm, opt => opt.MapFrom(src => src.Devices.SelectMany(d => d.SensorData).Max(sd => DateTime.Parse(sd.DateTime))))
                    .ForMember(dest => dest.TemperatureCount, opt => opt.MapFrom(src => src.Devices.SelectMany(d => d.SensorData).Count(sd => sd.SensorType == "TEMP")))
                    .ForMember(dest => dest.AverageTemperature, opt => opt.MapFrom(src => src.Devices.SelectMany(d => d.SensorData).Where(sd => sd.SensorType == "TEMP").Average(sd => sd.Value)))
                    .ForMember(dest => dest.HumidityCount, opt => opt.MapFrom(src => src.Devices.SelectMany(d => d.SensorData).Count(sd => sd.SensorType == "HUM")))
                    .ForMember(dest => dest.AverageHumidity, opt => opt.MapFrom(src => src.Devices.SelectMany(d => d.SensorData).Where(sd => sd.SensorType == "HUM").Average(sd => sd.Value)));
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task MergeIoTDataAsync_WhenGivenOneFile_ReturnsExpectedResults()
        {
            // Arrange
            var fileContent = File.ReadAllText(Path.Combine("Data", "DeviceDataFoo1.json"));
            var files = new List<IFormFile>
            {
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(fileContent)), 0, fileContent.Length, "DeviceDataFoo1", "DeviceDataFoo1.json")
            };
            var expectedResults = new List<IoTData>
            {
                new IoTData
                {
                    CompanyId = 1,
                    CompanyName = "Foo1",
                    DeviceId = 1,
                    DeviceName = "ABC-100",
                    FirstReadingDtm = new DateTime(2020, 8, 17, 10, 35, 0),
                    LastReadingDtm = new DateTime(2020, 8, 17, 10, 45, 0),
                    TemperatureCount = 3,
                    AverageTemperature = 23.15,
                    HumidityCount = 3,
                    AverageHumidity = 81.5
                }
            };

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IDeviceDataService>(new DeviceDataService(_mapper));
                });
            }).CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/IoT/MergeIoTDataAsync");
            var multipartContent = new MultipartFormDataContent();
            foreach (var formFile in files)
            {
                var fileContentBytes = await File.ReadAllBytesAsync(Path.Combine("Data", formFile.FileName));
                var byteArrayContent = new ByteArrayContent(fileContentBytes);
                byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                multipartContent.Add(byteArrayContent, "files", formFile.FileName);
            }
            request.Content = multipartContent;

            // Add API-SECRET-KEY header
            request.Headers.Add("API-SECRET-KEY", "J.G>JgVP<|;2ccj(9v,Fm/X]-d&9pC");

            // Act
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            var actualResults = JsonConvert.DeserializeObject<Response<List<IoTData>>>(responseContent).Data;

            // Assert
            Assert.Equal(expectedResults.Count, actualResults.Count);
            Assert.Equal(expectedResults.First().CompanyId, actualResults.First().CompanyId);
            Assert.Equal(expectedResults.First().CompanyName, actualResults.First().CompanyName);
            Assert.Equal(expectedResults.First().DeviceId, actualResults.First().DeviceId);
            Assert.Equal(expectedResults.First().DeviceName, actualResults.First().DeviceName);
            Assert.Equal(expectedResults.First().FirstReadingDtm, actualResults.First().FirstReadingDtm);
        }
    }
}