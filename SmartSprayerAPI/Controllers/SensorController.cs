using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSprayerAPI.Models;
using SmartSprayerAPI.Repositories;
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

        [HttpGet("{deviceId}")]
        public IActionResult GetByDeviceId(string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return BadRequest("deviceId is required");
            }

            var result = _service.GetByServiceId(deviceId);

            if (result.Count == 0)
            {
                return NotFound($"No data was found for devidceId: {deviceId}");
            }

            return Ok(result);
        }

        [HttpPut("{deviceId}")]
        public IActionResult UpdateSensorData(string deviceId, [FromBody] SensorData updatedData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(deviceId))
            {
                return BadRequest("deviceId is required");
            }

            var existing = _service.Update(deviceId, updatedData);

            if (existing == null) // If no data found
            {
                return NotFound($"Device {deviceId} not found");
            }

            _logger.LogInformation("Received sensor data from {deviceId}", existing.DeviceId);

            return Ok(updatedData);
        }

        [HttpDelete("{deviceId}")]
        public IActionResult DeleteSensorData(string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return BadRequest("deviceId is required");
            }

            var deleted = _service.Delete(deviceId);

            if (!deleted)
            {
                return NotFound($"Device {deviceId} not found");
            }

            return Ok(new { message = $"Device {deviceId} deleted" });
        }

        /// Alerts data
        [HttpGet("{deviceId}/alerts")]
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