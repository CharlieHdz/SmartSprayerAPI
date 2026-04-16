using SmartSprayerAPI.Models;

namespace SmartSprayerAPI.Interfaces
{
    public interface ISensorService
    {
        Task<List<SensorData>> GetAll();
        Task<List<SensorData>> GetByDeviceId(string deviceId);
        Task<SensorData?> GetLatestByDevice(string deviceId);

        Task Add(SensorData data);
        Task<SensorData?> Update(int id, SensorData data);
        Task<bool> Delete(int id);

        Task<List<Alert>> GetAlertsByDevice(string deviceId);
    }
}
