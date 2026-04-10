namespace SmartSprayerAPI.Models
{
    public class SensorData
    {
        //Data structure that the API will handle
        public string DeviceId { get; set; }
        public double Temperature { get; set; }
        public double Pressure { get; set; }
        public DateTime Timestamp { get; set; }


    }
}
