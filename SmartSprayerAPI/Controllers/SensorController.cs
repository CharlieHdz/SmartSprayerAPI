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
        public IActionResult PostSensorData([FromBody] SensorData sensorData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _service.Add(sensorData);

            _logger.LogInformation("Data received from {deviceId}", sensorData.DeviceId);

            return Ok(sensorData);
        }

        [HttpGet] // Returns Data
        public IActionResult GetSensorData()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("device/{deviceId}")]
        public IActionResult GetByDeviceId(string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return BadRequest("deviceId is required");
            }

            var result = _service.GetByDeviceId(deviceId);

            if (result.Count == 0)
            {
                return NotFound($"No data was found for devidceId: {deviceId}");
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSensorData(int id, [FromBody] SensorData updatedData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing = _service.Update(id, updatedData);

            if (existing == null) // If no data found
            {
                return NotFound($"Device id: {id} not found");
            }

            _logger.LogInformation("Received sensor data from {id}", existing.DeviceId);

            return Ok(updatedData);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSensorData(int id)
        {
            var deleted = _service.Delete(id);

            if (!deleted)
            {
                return NotFound($"Device {id} not found");
            }

            return Ok(new { message = $"Device {id} deleted" });
        }

        /// Alerts data
        [HttpGet("alerts/{deviceId}")]
        public IActionResult GetAlertsByDevice(string deviceId)
        {
            var alerts = _service.GetAlertsByDevice(deviceId);

            if (!alerts.Any())
            {
                return NotFound($"No device with {deviceId} ID found");
            }

            return Ok(alerts);
        }
    }
}