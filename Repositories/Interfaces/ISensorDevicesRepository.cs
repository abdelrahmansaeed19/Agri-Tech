using AgriculturalTech.API.Data.Models;

namespace AgriculturalTech.API.Repositories.Interfaces
{
    public interface ISensorDevicesRepository
    {
        public Task<SensorDevice> GetDeviceByUserIdAsync(string userid);

        public Task<bool> IsDevicePurchasedByUserIdAsync(string userid);

        public Task<List<SensorDevice>> GetDevicesByUserIdAsync(string userId);


    }
}
