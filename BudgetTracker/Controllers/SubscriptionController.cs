using BudgetTracker.DTO;
using BudgetTracker.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BudgetTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController (ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }


        [Authorize]
        [HttpPost("create-subscription")]
        public async Task<IActionResult> CreateSubscriptionAsync (SubscriptionRequest request)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _subscriptionService.CreateSubscriptionAsync(userId.Value, request);

            return Ok(response);
        }

        [Authorize]
        [HttpPatch("edit-subscription/{subscriptionId}")]
        public async Task<IActionResult> EditSubscriptionAsync (Guid subscriptionId, SubscriptionRequest request)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _subscriptionService.EditSubscriptionAsync(userId.Value, subscriptionId, request);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }


        [Authorize]
        [HttpDelete("cancel-subscription/{subscriptionId}")]
        public async Task<IActionResult> CancelSubscriptionAsync (Guid subscriptionId)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _subscriptionService.CancelSubscriptionAsync(userId.Value, subscriptionId);
            if (!response)
            {
                return NotFound();
            }
            return NoContent();
        }


        [Authorize]
        [HttpGet("get-subscriptions")]
        public async Task<IActionResult> GetSubscriptionsAsync()
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _subscriptionService.GetSubscriptionsAsync(userId.Value);
            return Ok(response);
        }



        private Guid? GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return null;
            }

            if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return null;
            }

            return userId;
        }
    }
}
