using AutoMapper;
using Domain.Devices.Domain;
using Domain.Devices.Models.Company;
using Domain.Devices.Models.Partners;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Application.Device.Data
{
    public class DeviceDataService: IDeviceDataService
    {
        private readonly IRepositoryService _repositoryService;
        private readonly IMapper _mapper;
        public DeviceDataService(IRepositoryService repositoryService, IMapper mapper)
        {
            _repositoryService = repositoryService;
            _mapper = mapper;
        }

        /// <summary>
        /// Merges the IoT data asynchronously.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <returns></returns>
        public async Task<List<IoTData>> MergeIoTDataAsync(IEnumerable<IFormFile> files)
        {
            // Make sure there is at leat one file
            if (files is null || !files.Any())
                throw new ArgumentNullException(nameof(files), "At least one file is required.");

            // Initialize the IoTData list
            var _iotDataList = new List<IoTData>();

            // Loop through the files
            foreach (var file in files)
            {
                // Read the file content
                string fileContent;
                using (var streamReader = new StreamReader(file.OpenReadStream()))
                {
                    fileContent = await streamReader.ReadToEndAsync();
                }

                // Check if the file contains partnerId
                if (fileContent.Contains("PartnerId"))
                {
                    var partnerData = JsonConvert.DeserializeObject<PartnerData>(fileContent);
                    var iotData = _mapper.Map<IoTData>(partnerData);
                    _iotDataList.Add(iotData);
                }
                // Check if the file contains companyId
                else if (fileContent.Contains("CompanyId"))
                {
                    var companyData = JsonConvert.DeserializeObject<CompanyData>(fileContent);
                    var iotData = _mapper.Map<IoTData>(companyData);
                    _iotDataList.Add(iotData);
                }
                else
                {
                    throw new ArgumentException("One or more files has an invalid file format");
                }
            }

            return _iotDataList;
        }
    }
}
