using SmartSprayerAPI.Models;

namespace SmartSprayerAPI.Repositories
{
    public static class SensorRepository
    {
        public static List<SensorData> Data = new List<SensorData>(); // Temporal database entity (simulating data storage)
        public static List<Alert> Alerts = new List<Alert>();         // Alert database entity
    }
}
