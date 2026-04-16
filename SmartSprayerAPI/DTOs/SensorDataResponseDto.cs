namespace SmartSprayerAPI.DTOs
{
    public class SensorDataResponseDto
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public double Temperature { get; set; }
        public double Pressure { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
