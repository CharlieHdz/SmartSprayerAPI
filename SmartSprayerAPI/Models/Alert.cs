namespace SmartSprayerAPI.Models
{
    public class Alert
    {
        public string DeviceId {  get; set; }
        public string Message { get; set; }
        public string Severity { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
