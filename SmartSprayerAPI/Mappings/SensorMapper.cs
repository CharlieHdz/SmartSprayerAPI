using SmartSprayerAPI.DTOs;
using SmartSprayerAPI.Models;

namespace SmartSprayerAPI.Mappings
{
    public static class SensorMapper
    {
        public static SensorDataResponseDto ToDto(SensorData x)
        {
            return new SensorDataResponseDto
            {
                Id = x.Id,
                DeviceId = x.DeviceId,
                Temperature = x.Temperature,
                Pressure = x.Pressure,
                Timestamp = x.Timestamp ?? DateTime.UtcNow
            };
        }
    }
}
