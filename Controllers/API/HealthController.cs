using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgriculturalTech.API.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        // cron-job.org will send a GET request here every 14 minutes
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            // Returns a simple 200 OK with no database calls and almost zero CPU usage
            return Ok(new { status = "awake", time = DateTime.UtcNow });
        }
    }
}
