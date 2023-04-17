using Application.Device.Data;
using AutoMapper;
using Domain.Devices.Domain;
using Domain.Devices.Models.Company;
using Domain.Devices.Models.Partners;
using Microsoft.AspNetCore.Http;
using System.Text;
using Xunit;

namespace Tests.UnitTests
{
    /// <summary>
    /// Iridium unit tests
    /// </summary>
    public class IoTDataServiceTests
    {
        private DeviceDataService _deviceDataService;
        private readonly IMapper _mapper;

        public IoTDataServiceTests()
        {
            // Initialize AutoMapper configuration
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

            _deviceDataService = new DeviceDataService(_mapper);
        }

        /// <summary>
        /// Merges the files and calculate values with no files throws argument null exception.
        /// </summary>
        [Fact]
        public async Task MergeFilesAndCalculateValues_WithNoFiles_ThrowsArgumentNullException()
        {
            // Arrange
            var files = new List<IFormFile>();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _deviceDataService.MergeIoTDataAsync(files));
        }

        /// <summary>
        /// Merges the files and calculate values with invalid file format throws argument exception.
        /// </summary>
        [Fact]
        public async Task MergeFilesAndCalculateValues_WithInvalidFileFormat_ThrowsArgumentException()
        {
            // Arrange
            var files = new List<IFormFile>()
        {
            new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("Invalid file content")), 0, 0, "data", "file.txt")
        };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _deviceDataService.MergeIoTDataAsync(files));
        }

        /// <summary>
        /// Merges the files and calculate values with valid files returns merged io t data.
        /// </summary>
        [Fact]
        public async Task MergeAndCalculateValues_WhenGivenOneFile_ReturnsExpectedResults()
        {
            // Arrange
            var fileName = "DeviceDataFoo2.json";
            var filePath = Path.Combine("Data", fileName);
            var fileContent = File.ReadAllText(filePath);
            var files = new List<IFormFile>
            {
                 new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(fileContent)), 0, fileContent.Length, "DeviceDataFoo2", fileName)
            };

            var expectedResults = new List<IoTData>
            {
                new IoTData
                {
                    CompanyId = 2,
                    CompanyName = "Foo2",
                    DeviceId = 1,
                    DeviceName = "XYZ-100",
                    FirstReadingDtm = new DateTime(2020, 8, 18, 10, 35, 0),
                    LastReadingDtm = new DateTime(2020, 8, 19, 10, 45, 0),
                    TemperatureCount = 6,
                    AverageTemperature = 38.15,
                    HumidityCount = 6,
                    AverageHumidity = 92
                }
            };

            // Act
            var actualResults = await _deviceDataService.MergeIoTDataAsync(files);

            // Assert
            Assert.Equal(expectedResults.Count, actualResults.Count);
            Assert.Equal(expectedResults.First().CompanyId, actualResults.First().CompanyId);
            Assert.Equal(expectedResults.First().CompanyName, actualResults.First().CompanyName);
            Assert.Equal(expectedResults.First().DeviceId, actualResults.First().DeviceId);
            Assert.Equal(expectedResults.First().DeviceName, actualResults.First().DeviceName);
            Assert.Equal(expectedResults.First().FirstReadingDtm, actualResults.First().FirstReadingDtm);
            Assert.Equal(expectedResults.First().LastReadingDtm, actualResults.First().LastReadingDtm);
            Assert.Equal(expectedResults.First().TemperatureCount, actualResults.First().TemperatureCount);
            Assert.Equal(expectedResults.First().AverageTemperature, actualResults.First().AverageTemperature);
            Assert.Equal(expectedResults.First().HumidityCount, actualResults.First().HumidityCount);
            Assert.Equal(expectedResults.First().AverageHumidity, actualResults.First().AverageHumidity);
        }
    }

}