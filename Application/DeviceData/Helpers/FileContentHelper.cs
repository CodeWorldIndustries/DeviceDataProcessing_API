using AutoMapper;
using Domain.Devices.Domain;
using Domain.Devices.Models.Company;
using Domain.Devices.Models.Partners;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Application.DeviceData.Helpers
{
    public static class FileContentHelper
    {
        /// <summary>
        /// Reads the file content asynchronously.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static async Task<string> ReadFileContentAsync(IFormFile file)
        {
            using var streamReader = new StreamReader(file.OpenReadStream());
            var fileContent = await streamReader.ReadToEndAsync();
            return fileContent;
        }

        /// <summary>
        /// Deserializes the file content.
        /// </summary>
        /// <param name="fileContent">Content of the file.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">One or more files has an invalid file format</exception>
        public static IoTData DeserializeFileContent(string fileContent, IMapper mapper)
        {
            if (fileContent.Contains("PartnerId"))
            {
                var partnerData = JsonConvert.DeserializeObject<PartnerData>(fileContent);
                return mapper.Map<IoTData>(partnerData);
            }

            if (fileContent.Contains("CompanyId"))
            {
                var companyData = JsonConvert.DeserializeObject<CompanyData>(fileContent);
                return mapper.Map<IoTData>(companyData);
            }

            throw new ArgumentException("One or more files has an invalid file format");
        }
    }
}
