using SmartSprayerAPI.Models;
using SmartSprayerAPI.Data;

namespace SmartSprayerAPI.Services
{
    public class SensorService
    {
        private readonly AppDbContext _context;

        public SensorService(AppDbContext context)
        {
            _context = context;
        }

        public List<SensorData> GetAll() => _context.SensorData.ToList();

        public List<SensorData> GetByDeviceId(string deviceId)
        {
            return _context.SensorData.Where(x => x.DeviceId == deviceId).ToList();
        }
        public void Add(SensorData data)
        {
            data.Timestamp = DateTime.UtcNow;

            _context.SensorData.Add(data);
            _context.SaveChanges();

            EvaluateRules(data);
        }

        public SensorData Update(int id, SensorData updatedData)
        {
            var existing = _context.SensorData.FirstOrDefault(x => x.Id == id);

            if (existing == null)
            {
                return null;
            }

            existing.Temperature = updatedData.Temperature;
            existing.Pressure = updatedData.Pressure;
            existing.Timestamp = DateTime.UtcNow;

            _context.SaveChanges();

            return existing;
        }

        public bool Delete(int id)
        {
            var existing = _context.SensorData.FirstOrDefault(x => x.Id == id);

            if (existing == null)
            {
                return false;
            }

            _context.SensorData.Remove(existing);
            _context.SaveChanges();

            return true;
        }

        public List<Alert> GetAlertsByDevice(string deviceId)
        {
            return _context.Alerts.Where(a => a.DeviceId == deviceId).ToList();
        }

        private void CreateAlert(SensorData data, string message, string severity)
        {
            var recentAlert = _context.Alerts.Where(a => a.DeviceId == data.DeviceId && a.Message == message)
                                             .OrderByDescending(a => a.Timestamp)
                                             .FirstOrDefault();

            if (recentAlert != null && (DateTime.UtcNow - recentAlert.Timestamp).TotalSeconds < 30)
            {
                return; // Debouncing logic, post a new DTC alert every 30 seconds
            }

            var alert = new Alert
            {
                DeviceId = data.DeviceId,
                Message = message,
                Severity = severity,
                Timestamp = DateTime.UtcNow
            };

            _context.Alerts.Add(alert);
            _context.SaveChanges();
        }

        private void EvaluateRules(SensorData data)
        {
            CheckPressure(data);
            CheckTemperature(data);
        }

        private void CheckPressure(SensorData data)
        {
            if (data.Pressure > 150)
                CreateAlert(data, "Critical pressure detected", "Critical");
            else if (data.Pressure > 100)
                CreateAlert(data, "High pressure detected", "High");
        }

        private void CheckTemperature(SensorData data)
        {
            if (data.Temperature > 90)
                CreateAlert(data, "High temperature detected", "High");
        }
    }
}
