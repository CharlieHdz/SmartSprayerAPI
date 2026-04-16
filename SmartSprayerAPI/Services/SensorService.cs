using Microsoft.EntityFrameworkCore;
using SmartSprayerAPI.Data;
using SmartSprayerAPI.Interfaces;
using SmartSprayerAPI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SmartSprayerAPI.Services
{
    public class SensorService : ISensorService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SensorService> _logger;

        public SensorService(AppDbContext context, ILogger<SensorService> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<List<SensorData>> GetAll()
        {
            return await _context.SensorData.ToListAsync();
        }

        public async Task<List<SensorData>> GetByDeviceId(string deviceId)
        {
            return await _context.SensorData.Where(x => x.DeviceId == deviceId).ToListAsync();
        }

        public async Task<SensorData?> GetLatestByDevice(string deviceId)
        {
            return await _context.SensorData.Where(x => x.DeviceId == deviceId)
                                            .OrderByDescending(x => x.Timestamp)
                                            .FirstOrDefaultAsync();
        }

        public async Task Add(SensorData data)
        {
            data.Timestamp = DateTime.UtcNow;

            await _context.SensorData.AddAsync(data);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Sensor data stored for {deviceId} at {time}", data.DeviceId, data.Timestamp);

            await EvaluateRules(data);
        }

        public async Task<SensorData?> Update(int id, SensorData updatedData)
        {
            var existing = await _context.SensorData.FindAsync(id);

            if (existing == null)
            {
                return null;
            }

            _logger.LogInformation($"Updated Temperature from {existing.Temperature} to {updatedData.Temperature} and Pressure from {existing.Pressure} to {updatedData.Pressure}");

            existing.Temperature = updatedData.Temperature;
            existing.Pressure = updatedData.Pressure;
            existing.Timestamp = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return existing;
        }

        public async Task<bool> Delete(int id)
        {
            var existing = await _context.SensorData.FindAsync(id);

            if (existing == null)
            {
                return false;
            }

            _context.SensorData.Remove(existing);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Deleted element with ID: {id} from dbc");

            return true;
        }

        public async Task<List<Alert>> GetAlertsByDevice(string deviceId)
        {
            return await _context.Alerts.Where(a => a.DeviceId == deviceId).ToListAsync();
        }

        private async Task CreateAlert(SensorData data, string message, string severity)
        {
            var cutoff = DateTime.UtcNow.AddSeconds(-30);

            var recentAlert = await _context.Alerts.Where(a => a.DeviceId == data.DeviceId && a.Message == message && a.Timestamp >= cutoff)
                                                   .OrderByDescending(a => a.Timestamp)
                                                   .FirstOrDefaultAsync();

            if (recentAlert != null)
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

            await _context.Alerts.AddAsync(alert);
            await _context.SaveChangesAsync();
        }

        private async Task EvaluateRules(SensorData data)
        {
            await CheckPressure(data);
            await CheckTemperature(data);
        }

        private async Task CheckPressure(SensorData data)
        {
            if (data.Pressure > 150)
                await CreateAlert(data, "Critical pressure detected", "Critical");
            else if (data.Pressure > 100)
                await CreateAlert(data, "High pressure detected", "High");
        }

        private async Task CheckTemperature(SensorData data)
        {
            if (data.Temperature > 90)
                await CreateAlert(data, "High temperature detected", "High");
        }
    }
}
