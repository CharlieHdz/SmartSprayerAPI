using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmartSprayerAPI.DTOs;
using SmartSprayerAPI.Mappings;
using SmartSprayerAPI.Models;
using SmartSprayerAPI.Interfaces;
using SmartSprayerAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace SmartSprayerAPI.Controllers
{
    [Authorize]             // Default all endpoints to be protected
    [ApiController]         // Means = API REST
    [Route("sensor-data")]  // Base URL
    public class SensorController : ControllerBase
    {
        private readonly ILogger<SensorController> _logger;
        private readonly ISensorService _service;

        public SensorController(ISensorService service, ILogger<SensorController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost] // Receives Data
        public async Task<IActionResult> PostSensorData([FromBody] SensorDataCreateDto sensorDataDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = new SensorData
            {
                DeviceId = sensorDataDto.DeviceId,
                Temperature = sensorDataDto.Temperature,
                Pressure = sensorDataDto.Pressure
            };

            await _service.Add(model);

            _logger.LogInformation("Data received from {deviceId}", model.DeviceId);

            return Ok(model);
        }

        [AllowAnonymous]
        [HttpGet] // Returns Data
        public async Task<IActionResult> GetSensorData()
        {
            var data = await _service.GetAll();

            var response = data.Select(SensorMapper.ToDto).ToList();

            return Ok(response);
        }

        [AllowAnonymous]
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

            var response = result.Select(SensorMapper.ToDto).ToList();

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("latest/{deviceId}")]
        public async Task<IActionResult> GetLatest(string deviceId)
        {
            var latest = await _service.GetLatestByDevice(deviceId);

            if (latest == null)
                return NotFound();

            return Ok(latest);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSensorData(int id, [FromBody] SensorDataUpdateDto updatedDataUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = new SensorData
            {
                Temperature = updatedDataUpdateDto.Temperature,
                Pressure = updatedDataUpdateDto.Pressure
            };

            var existing = await _service.Update(id, model);

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
        [AllowAnonymous]
        [HttpGet("alerts/{deviceId}")]
        public async Task<IActionResult> GetAlertsByDevice(string deviceId)
        {
            var alerts = await _service.GetAlertsByDevice(deviceId);

            if (!alerts.Any())
            {
                return NotFound($"No alerts found for device ID: {deviceId}");
            }

            var response = alerts.Select(AlertMapper.ToDto).ToList();

            return Ok(response);
        }
    }
}