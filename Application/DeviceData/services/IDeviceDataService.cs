using Application.DeviceData.Contracts;
using Domain.Devices.Domain;
using Microsoft.AspNetCore.Http;

namespace Application.DeviceData.services
{
    public interface IDeviceDataService
    {
        Task<List<IoTData>> GetDataAsync(GetDataSummaryRequest request);
        Task<List<IoTData>> MergeIoTDataAsync(IEnumerable<IFormFile> files);
    }
}
