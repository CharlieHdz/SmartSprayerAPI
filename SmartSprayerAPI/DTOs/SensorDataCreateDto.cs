namespace SmartSprayerAPI.DTOs
{
    public class SensorDataCreateDto
    {
        public string DeviceId { get; set; }
        public double Temperature { get; set; }
        public double Pressure { get; set; }
    }
}
