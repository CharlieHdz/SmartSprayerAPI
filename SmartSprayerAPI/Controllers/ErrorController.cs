using Microsoft.AspNetCore.Mvc;

namespace SmartSprayerAPI.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult HandleError()
        {
            return Problem("An unexpected error occurred.");
        }
    }
}
