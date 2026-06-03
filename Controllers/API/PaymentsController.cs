using AgriculturalTech.API.DTOs;
using AgriculturalTech.API.Services.Interfaces;
using AgriculturalTech.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgriculturalTech.API.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IStripePaymentService _stripePaymentService;
        private readonly IUserSubscriptionRepository _userSubscriptionRepository;
        private readonly ISensorDevicesRepository _sensorDevicesRepository;

        public PaymentsController(IStripePaymentService stripePaymentService, IUserSubscriptionRepository userSubscriptionRepository, ISensorDevicesRepository sensorDevicesRepository)
        {
            _stripePaymentService = stripePaymentService;
            _userSubscriptionRepository = userSubscriptionRepository;
            _sensorDevicesRepository = sensorDevicesRepository;
        }

        [HttpPost("subscribe")]
        public async Task<ActionResult<ApiResponse<string>>> Subscribe()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var sub = await _userSubscriptionRepository.GetSubscriptionByUserIdAsync(userId);

            if(sub != null)
            {

                if (sub.CancelAtPeriodEnd)
                {
                    return BadRequest(ApiResponse<string>.ErrorResponse($"User already has an active subscription but will end at {sub.CurrentPeriodEnd} if not updated by user"));
                }

                return BadRequest(ApiResponse<string>.ErrorResponse("User already has an active subscription."));
            }

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

            var sub = await _userSubscriptionRepository.GetSubscriptionByUserIdAsync(userId);

            if (sub == null)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse("User is not subscriped."));
            }

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

        [HttpPost("purchase-kit")]
        public async Task<ActionResult<ApiResponse<string>>> PurchaseKit()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userId == null || userEmail == null)
            {
                return Unauthorized(ApiResponse<string>.ErrorResponse("User information is missing."));
            }

            if(await _sensorDevicesRepository.IsDevicePurchasedByUserIdAsync(userId))
            {
                return BadRequest(ApiResponse<string>.ErrorResponse("User has already purchased the kit."));
            }

            try
            {
                var resultUrl = await _stripePaymentService.CreateKitCheckoutSessionAsync(userId, userEmail);

                return Ok(ApiResponse<string>.SuccessResponse(resultUrl, "Kit purchase checkout session created successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse("Failed to create kit purchase checkout session.", new List<string> { ex.Message }));
            }
        }
    }
}
