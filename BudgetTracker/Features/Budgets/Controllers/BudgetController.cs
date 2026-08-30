using BudgetTracker.Features.Budgets.DTOs;
using BudgetTracker.Features.Budgets.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace BudgetTracker.Features.Budgets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly IWriteBudgetService _writeBudgetService;
        private readonly IReadBudgetService _readBudgetService;

        public BudgetController(IReadBudgetService readBudgetService, IWriteBudgetService writeBudgetService)
        {
            _writeBudgetService = writeBudgetService;
            _readBudgetService = readBudgetService;
        }


        [EnableRateLimiting("General")]
        [Authorize]
        [HttpPost("create-budget")]
        public async Task<IActionResult> CreateBudgetAsync(BudgetRequest request)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _writeBudgetService.CreateBudgetAsync(userId.Value, request);
            return Ok(response);
        }


        [EnableRateLimiting("General")]
        [Authorize]
        [HttpPatch("edit-budget/{month}")]
        public async Task<IActionResult> EditBudgetAsync( DateOnly month, BudgetRequest request)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _writeBudgetService.EditBudgetAsync( userId.Value, month, request);
            return Ok(response);
        }


        [EnableRateLimiting("General")]
        [Authorize]
        [HttpGet("get-budget/{month}")]
        public async Task<IActionResult> GetBudgetAsync(DateOnly month)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _readBudgetService.GetBudgetAsync( userId.Value, month);
            return Ok(response);
        }


        [EnableRateLimiting("General")]
        [Authorize]
        [HttpGet("get-all-budgets")]
        public async Task<IActionResult> GetAllBudgetsAsync()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _readBudgetService.GetAllBudgetsAsync(
                userId.Value);
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
