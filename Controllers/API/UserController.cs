using AgriculturalTech.API.Data.Models;
using AgriculturalTech.API.DTOs;
using AgriculturalTech.API.Repositories.Interfaces;
using AgriculturalTech.API.Services.Implementations;
using AgriculturalTech.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IUserSubscriptionRepository userSubscriptionRepository, UserManager<ApplicationUser> userManager)
        {
            _userSubscriptionRepository = userSubscriptionRepository;
            _userManager = userManager;
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

        [HttpPost("update-prefarred-language")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateUserPreferredLanguage([FromBody] UpdatePreferredLanguageDto updatePreferredLanguageDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(ApiResponse<string>.ErrorResponse("User not found"));
            }

            user.PreferredLanguage = updatePreferredLanguageDto.PreferredLanguage;

            try
            {
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok(ApiResponse<string>.SuccessResponse("Preferred language updated successfully"));
                }
                else
                {
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    return BadRequest(ApiResponse<string>.ErrorResponse($"Failed to update preferred language: {errors}"));
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse($"An error occurred while updating preferred language: {ex.Message}"));
            }
        }
    }
}
