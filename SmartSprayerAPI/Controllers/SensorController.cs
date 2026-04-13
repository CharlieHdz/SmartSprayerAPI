using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmartSprayerAPI.Models;
using SmartSprayerAPI.Services;

namespace SmartSprayerAPI.Controllers
{
    [ApiController]         // Means = API REST
    [Route("sensor-data")]  // Base URL
    public class SensorController : ControllerBase
    {
        private readonly ILogger<SensorController> _logger;
        private readonly SensorService _service;

        public SensorController(SensorService service, ILogger<SensorController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost] // Receives Data
        public async Task<IActionResult> PostSensorData([FromBody] SensorData sensorData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.Add(sensorData);

            _logger.LogInformation("Data received from {deviceId}", sensorData.DeviceId);

            return Ok(sensorData);
        }

        [HttpGet] // Returns Data
        public async Task<IActionResult> GetSensorData()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet("device/{deviceId}")]
        public async Task<IActionResult> GetByDeviceId(string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return BadRequest("deviceId is required");
            }

            var result = await _service.GetByDeviceId(deviceId);

            if (result.Count == 0)
            {
                return NotFound($"No data was found for devidceId: {deviceId}");
            }

            return Ok(result);
        }

        [HttpGet("latest/{deviceId}")]
        public async Task<IActionResult> GetLatest(string deviceId)
        {
            var latest = await _service.GetLatestByDevice(deviceId);

            if (latest == null)
                return NotFound();

            return Ok(latest);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSensorData(int id, [FromBody] SensorData updatedData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing = await _service.Update(id, updatedData);

            if (existing == null) // If no data found
            {
                return NotFound($"Device id: {id} not found");
            }

            _logger.LogInformation("Updated sensor data for Id {id}", id);

            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSensorData(int id)
        {
            var deleted = await _service.Delete(id);

            if (!deleted)
            {
                return NotFound($"Device {id} not found");
            }

            return Ok(new { message = $"Device {id} deleted" });
        }

        /// Alerts data
        [HttpGet("alerts/{deviceId}")]
        public async Task<IActionResult> GetAlertsByDevice(string deviceId)
        {
            var alerts = await _service.GetAlertsByDevice(deviceId);

            if (!alerts.Any())
            {
                return NotFound($"No alerts found for device ID: {deviceId}");
            }

            return Ok(alerts);
        }
    }
}