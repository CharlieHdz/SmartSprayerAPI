using System.ComponentModel.DataAnnotations;

namespace SmartSprayerAPI.Models
{
    public class Alert
    {
        [Key]
        public int Id { get; set; }
        public string DeviceId {  get; set; }
        public string Message { get; set; }
        public string Severity { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
