using Domain.Devices.Domain;
using Microsoft.AspNetCore.Http;

namespace Application.Device.Data
{
    public interface IDeviceDataService
    {
        Task<List<IoTData>> MergeIoTDataAsync(IEnumerable<IFormFile> files);
    }
}
