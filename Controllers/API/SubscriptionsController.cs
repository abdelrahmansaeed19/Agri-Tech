using AgriculturalTech.API.DTOs;
using AgriculturalTech.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgriculturalTech.API.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly IStripePaymentService _stripePaymentService;

        public SubscriptionsController(IStripePaymentService stripePaymentService)
        {
            _stripePaymentService = stripePaymentService;
        }

        [HttpPost("subscribe")]
        public async Task<ActionResult<ApiResponse<string>>> Subscribe()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            try
            {
                var resultUrl = await _stripePaymentService.CreateSubscriptionCheckoutSessionAsync(userId, userEmail);

                return Ok(ApiResponse<string>.SuccessResponse(resultUrl, "Subscription checkout session created successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse("Failed to create subscription checkout session.", new List<string> { ex.Message }));
            }
        }

        [HttpPost("manage-billing")]
        public async Task<ActionResult<ApiResponse<string>>> ManageBilling()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                var resultUrl = await _stripePaymentService.CreateCustomerPortalSessionAsync(userId);

                return Ok(ApiResponse<string>.SuccessResponse(resultUrl, "Billing portal session created successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse("Failed to create billing portal session.", new List<string> { ex.Message }));
            }
        }
    }
}
