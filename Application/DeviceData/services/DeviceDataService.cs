using Application.DeviceData.Contracts;
using Application.DeviceData.Helpers;
using AutoMapper;
using Domain.Devices.Domain;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;

namespace Application.DeviceData.services
{
    /// <summary>
    /// The Device Data Service
    /// </summary>
    /// <seealso cref="IDeviceDataService" />
    public class DeviceDataService : IDeviceDataService
    {
        private readonly IRepositoryService _repositoryService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceDataService"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        public DeviceDataService(IRepositoryService repositoryService, IMapper mapper)
        {
            _repositoryService = repositoryService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the data asynchronously.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<List<IoTData>> GetDataAsync(GetDataSummaryRequest request)
        {
            var ioTData = await _repositoryService.GetAsync<IoTData>(d => d.CreatedDate >= request.From && d.CreatedDate < request.To);
            return ioTData.OrderByDescending(x => x.CreatedDate).ToList();
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
                    var deserializedIoTData = FileContentHelper.DeserializeFileContent(fileContent, _mapper);
                    var iotData = await _repositoryService.CreateAsync(IoTData.Create(deserializedIoTData));
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
