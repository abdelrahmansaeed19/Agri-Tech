using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AgriculturalTech.API.Services.Interfaces;

namespace AgriculturalTech.API.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly IStripeWebhookService _webhookService;
        private readonly ILogger<WebhookController> _logger;

        public WebhookController(IStripeWebhookService webhookService)
        {
            _webhookService = webhookService;
        }

        [HttpPost("stripe")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var signatureHeader = Request.Headers["Stripe-Signature"];

            _logger.LogInformation("==================================================");
            _logger.LogInformation("DEBUG: Received Stripe webhook event: {EventData}", json);
            _logger.LogInformation("==================================================");

            try
            {
                await _webhookService.ProcessStripeEventAsync(json, signatureHeader);

                _logger.LogInformation("==================================================");
                _logger.LogInformation("Successfully processed Stripe webhook event.");
                _logger.LogInformation("==================================================");

                return Ok(); // You MUST return 200 OK so Stripe knows you received it
            }
            catch (Exception)
            {
                return BadRequest(); // Return 400 if validation fails
            }
        }
    }
}
