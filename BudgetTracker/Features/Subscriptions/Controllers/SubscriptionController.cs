using BudgetTracker.Features.Subscriptions.DTOs;
using BudgetTracker.Features.Subscriptions.Interfaces;
using BudgetTracker.Features.Subscriptions.Models;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> CreateSubscriptionAsync (SubscriptionRequest request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _writeSubscriptionService.CreateSubscriptionAsync(userId.Value, request, cancellationToken);
            return response.Success ? CreatedAtAction(nameof(GetSubscriptionsAsync), new { }, response.Response) : MapError(response);
        }

        [EnableRateLimiting("General")]
        [Authorize]
        [HttpPatch("edit-subscription/{subscriptionId}")]
        public async Task<IActionResult> EditSubscriptionAsync (Guid subscriptionId, SubscriptionRequest request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _writeSubscriptionService.EditSubscriptionAsync(userId.Value, subscriptionId, request, cancellationToken);

            return response.Success ? Ok(response.Response) : MapError(response);
        }


        [EnableRateLimiting("General")]
        [Authorize]
        [HttpPatch("cancel-subscription/{subscriptionId}")]
        public async Task<IActionResult> CancelSubscriptionAsync (Guid subscriptionId, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _writeSubscriptionService.CancelSubscriptionAsync(userId.Value, subscriptionId, cancellationToken);
            return response.Success ? NoContent() : MapError(response);
        }


        [EnableRateLimiting("General")]
        [Authorize]
        [HttpPatch("activate-subscription/{subscriptionId}")]
        public async Task<IActionResult> ActivateSubscriptionAsync(Guid subscriptionId, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _writeSubscriptionService.ActivateSubscriptionAsync(userId.Value, subscriptionId, cancellationToken);
            return response.Success ? NoContent() : MapError(response);

        }


        [EnableRateLimiting("General")]
        [Authorize]
        [HttpDelete("delete-subscription/{subscriptionId}")]
        public async Task<IActionResult> DeleteSubscriptionAsync(Guid subscriptionId, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _writeSubscriptionService.DeleteSubscriptionAsync(userId.Value, subscriptionId, cancellationToken);
            return response.Success ? NoContent() : MapError(response);

        }


        [EnableRateLimiting("General")]
        [Authorize]
        [HttpGet("get-subscriptions")]
        public async Task<IActionResult> GetSubscriptionsAsync(CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _readSubscriptionService.GetSubscriptionsAsync(userId.Value, cancellationToken);
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

        private IActionResult MapError(SubscriptionResult result)
        {
            return result.ErrorType switch
            {
                SubscriptionErrorType.SubscriptionNotFound => NotFound(result.ErrorMessage),
                _ => BadRequest(result.ErrorMessage)
            };
        }
    }
}
