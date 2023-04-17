using Domain.Devices.Domain;
using Microsoft.AspNetCore.Http;

namespace Application.DeviceData.services
{
    public interface IDeviceDataService
    {
        Task<List<IoTData>> MergeIoTDataAsync(IEnumerable<IFormFile> files);
    }
}
