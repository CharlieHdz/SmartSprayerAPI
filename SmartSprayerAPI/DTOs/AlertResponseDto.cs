namespace SmartSprayerAPI.DTOs
{
    public class AlertResponseDto
    {
        public string? DeviceId { get; set; }
        public string? Message { get; set; }
        public string? Severity { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
