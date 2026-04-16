using SmartSprayerAPI.DTOs;
using SmartSprayerAPI.Models;

namespace SmartSprayerAPI.Mappings
{
    public static class AlertMapper
    {
        public static AlertResponseDto ToDto(Alert x)
        {
            return new AlertResponseDto
            {
                DeviceId = x.DeviceId,
                Severity = x.Severity,
                Message = x.Message,
                Timestamp = x.Timestamp.Value
            };
        }
    }
}
