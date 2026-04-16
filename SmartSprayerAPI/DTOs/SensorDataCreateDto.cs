using System.ComponentModel.DataAnnotations;

namespace SmartSprayerAPI.DTOs
{
    public class SensorDataCreateDto
    {
        [Required]
        
        [Range(-50, 150)]
        public double Temperature { get; set; }

        [Range(0, 500)]
        public double Pressure { get; set; }
    }
}
