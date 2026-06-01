using AgriculturalTech.API.Data.Models;
using AgriculturalTech.API.DTOs;
using AgriculturalTech.API.Repositories.Interfaces;
using AgriculturalTech.API.Services.Implementations;
using AgriculturalTech.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace AgriculturalTech.API.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserSubscriptionRepository _userSubscriptionRepository;

        public UserController(IUserSubscriptionRepository userSubscriptionRepository)
        {
            _userSubscriptionRepository = userSubscriptionRepository;
        }

        [HttpGet("Subscription")]
        public async Task<ActionResult<ApiResponse<SubscriptionDto>>> GetUserSubscription()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var subscription = await _userSubscriptionRepository.GetSubscriptionByUserIdAsync(userId);

            if (subscription == null)
            {
                return NotFound(ApiResponse<SubscriptionDto>.ErrorResponse("No subscription found for the user"));
            }

            var subscriptionDto = new SubscriptionDto
            {
                Status = subscription.SubscriptionStatus == "active",
                CurrentPeriodStart = subscription.CurrentPeriodStart,
                CurrentPeriodEnd = subscription.CurrentPeriodEnd,
                IsCancelAtPeriodEnd = subscription.CancelAtPeriodEnd
            };

            return Ok(ApiResponse<SubscriptionDto>.SuccessResponse(subscriptionDto));
        }

    }
}
