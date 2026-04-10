using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSprayerAPI.Models;
using SmartSprayerAPI.Repositories;

namespace SmartSprayerAPI.Controllers
{
    [ApiController] // Means = API REST
    [Route("sensor-data")] // Base URL
    public class SensorController : ControllerBase
    {
        [HttpPost] // Receives Data
        public IActionResult PostSensorData([FromBody] SensorData sensorData)
        {
            SensorRepository.Data.Add(sensorData);
            return Ok(new { message = "Data received" });
        }

        [HttpGet] // Returns Data
        public IActionResult GetSensorData()
        {
            return Ok(SensorRepository.Data);
        }

        [HttpGet("{deviceId}")]
        public IActionResult GetByDeviceId(string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return BadRequest("deviceId is required");
            }

            var result = SensorRepository.Data.Where(x => x.DeviceId == deviceId).ToList(); // Using Language Intrated Query (LINQ)

            if (result.Count == 0)
            {
                return NotFound($"No data was found for devidceId: {deviceId}");
            }

            return Ok(result);
        }

        [HttpPut("{deviceId}")]
        public IActionResult UpdateSensorData(string deviceId, [FromBody] SensorData updatedData)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return BadRequest("deviceId is required");
            }

            var existing = SensorRepository.Data.FirstOrDefault(x => x.DeviceId == deviceId); //Look for a single element ID

            if (existing == null) // If no data found
            {
                return NotFound($"Device {deviceId} not found");
            }

            existing.Temperature = updatedData.Temperature;
            existing.Pressure = updatedData.Pressure;
            existing.Timestamp = updatedData.Timestamp;

            return Ok(existing);
        }

        [HttpDelete("{deviceID}")]
        public IActionResult DeleteSensorData(string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return BadRequest("deviceId is required");
            }

            var existing = SensorRepository.Data.FirstOrDefault(x => x.DeviceId == deviceId);

            if (existing == null)
            {
                return NotFound($"Device {deviceId} not found");
            }

            SensorRepository.Data.Remove(existing);

            return Ok(new { message = $"Device {deviceId} deleted" });
        }
    }
}
