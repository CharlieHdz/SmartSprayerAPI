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

            EvaluateRules(data);
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

        public List<Alert> GetAlerts()
        {
            return SensorRepository.Alerts;
        }

        private void CreateAlert(SensorData data, string message, string severity)
        {
            var alert = new Alert
            {
                DeviceId = data.DeviceId,
                Message = message,
                Severity = severity,
                Timestamp = DateTime.UtcNow
            };

            SensorRepository.Alerts.Add(alert);
        }

        private void EvaluateRules(SensorData data)
        {
            if (data.Pressure > 150)
            {
                CreateAlert(data, "Critical pressure detected", "Critical");
            }
            else if (data.Pressure > 100)
            {
                CreateAlert(data, "High pressure detected", "High");
            }

            if (data.Temperature > 90)
            {
                CreateAlert(data, "High temperature detected", "High");
            }
        }
    }
}
