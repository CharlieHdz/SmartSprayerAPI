using Microsoft.EntityFrameworkCore;
using SmartSprayerAPI.Models;

namespace SmartSprayerAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<SensorData> SensorData { get; set; }
        public DbSet<Alert> Alerts { get; set; }
    }
}
