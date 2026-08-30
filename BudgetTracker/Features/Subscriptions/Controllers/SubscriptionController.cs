using BudgetTracker.Features.Subscriptions.DTOs;
using BudgetTracker.Features.Subscriptions.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace BudgetTracker.Features.Subscriptions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly IReadSubscriptionService _readSubscriptionService;
        private readonly IWriteSubscriptionService _writeSubscriptionService;

        public SubscriptionController (IReadSubscriptionService readSubscriptionService, IWriteSubscriptionService writeSubscriptionService)
        {
            _writeSubscriptionService = writeSubscriptionService;
            _readSubscriptionService = readSubscriptionService;
        }

        [EnableRateLimiting("General")]
        [Authorize]
        [HttpPost("create-subscription")]
        public async Task<IActionResult> CreateSubscriptionAsync (SubscriptionRequest request)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _writeSubscriptionService.CreateSubscriptionAsync(userId.Value, request);
            return Ok(response);
        }

        [EnableRateLimiting("General")]
        [Authorize]
        [HttpPatch("edit-subscription/{subscriptionId}")]
        public async Task<IActionResult> EditSubscriptionAsync (Guid subscriptionId, SubscriptionRequest request)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _writeSubscriptionService.EditSubscriptionAsync(userId.Value, subscriptionId, request);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }


        [EnableRateLimiting("General")]
        [Authorize]
        [HttpDelete("cancel-subscription/{subscriptionId}")]
        public async Task<IActionResult> CancelSubscriptionAsync (Guid subscriptionId)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _writeSubscriptionService.CancelSubscriptionAsync(userId.Value, subscriptionId);
            return Ok(response);
        }


        [EnableRateLimiting("General")]
        [Authorize]
        [HttpDelete("activate-subscription/{subscriptionId}")]
        public async Task<IActionResult> ActivateSubscriptionAsync(Guid subscriptionId)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _writeSubscriptionService.ActivateSubscriptionAsync(userId.Value, subscriptionId);
            return Ok(response);
        }


        [EnableRateLimiting("General")]
        [Authorize]
        [HttpDelete("delete-subscription/{subscriptionId}")]
        public async Task<IActionResult> DeleteSubscriptionAsync(Guid subscriptionId)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _writeSubscriptionService.DeleteSubscriptionAsync(userId.Value, subscriptionId);
            return Ok(response);
        }


        [EnableRateLimiting("General")]
        [Authorize]
        [HttpGet("get-subscriptions")]
        public async Task<IActionResult> GetSubscriptionsAsync()
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _readSubscriptionService.GetSubscriptionsAsync(userId.Value);
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
