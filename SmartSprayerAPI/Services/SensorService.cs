using SmartSprayerAPI.Repositories;
using SmartSprayerAPI.Models;

namespace SmartSprayerAPI.Services
{
    public class SensorService
    {
        public List<SensorData> GetAll() => SensorRepository.Data;

        public List<SensorData> GetByServiceId(string deviceId)
        {
            return SensorRepository.Data.Where(x => x.DeviceId == deviceId).ToList();
        }
        public void Add(SensorData data)
        {
            data.Timestamp = DateTime.UtcNow;

            SensorRepository.Data.Add(data);
        }

        public SensorData Update(string deviceId, SensorData updatedData)
        {
            var existing = SensorRepository.Data.FirstOrDefault(x => x.DeviceId == deviceId);

            if (existing == null)
            {
                return null;
            }

            existing.Temperature = updatedData.Temperature;
            existing.Pressure = updatedData.Pressure;
            existing.Timestamp = DateTime.UtcNow;

            return existing;
        }

        public bool Delete(string deviceId)
        {
            var existing = SensorRepository.Data.FirstOrDefault(x => x.DeviceId == deviceId);

            if (existing == null)
            {
                return false;
            }

            SensorRepository.Data.Remove(existing);

            return true;
        }
    }
}
