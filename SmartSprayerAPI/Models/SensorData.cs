using System.ComponentModel.DataAnnotations;

namespace SmartSprayerAPI.Models
{
    public class SensorData
    {
        [Key]
        public int Id { get; set; }

        [Required] //Data structure that the API will handle
        public string? DeviceId { get; set; }

        [Range(-50, 150)]
        public double Temperature { get; set; }

        [Range(0, 500)]
        public double Pressure { get; set; }

        public DateTime? Timestamp { get; set; }
    }
}
