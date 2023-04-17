using Application.DeviceData.Helpers;
using AutoMapper;
using Domain.Devices.Domain;
using Microsoft.AspNetCore.Http;

namespace Application.DeviceData.services
{
    /// <summary>
    /// The Device Data Service
    /// </summary>
    /// <seealso cref="IDeviceDataService" />
    public class DeviceDataService : IDeviceDataService
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceDataService"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        public DeviceDataService(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Merges the IoT data asynchronously.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <returns>List of IoTData</returns>
        public async Task<List<IoTData>> MergeIoTDataAsync(IEnumerable<IFormFile> files)
        {
            // Validate files
            if (files is null || !files.Any())
                throw new ArgumentNullException(nameof(files), "At least one file is required.");

            // Create list
            var iotDataList = new List<IoTData>();

            // Read files
            foreach (var file in files)
            {
                try
                {
                    var fileContent = await FileContentHelper.ReadFileContentAsync(file);
                    var iotData = FileContentHelper.DeserializeFileContent(fileContent, _mapper);
                    iotDataList.Add(iotData);
                }
                catch
                {
                    continue;
                }
            }

            // Return list
            return iotDataList.OrderBy(x => x.CompanyId).ToList();
        }
    }
}
