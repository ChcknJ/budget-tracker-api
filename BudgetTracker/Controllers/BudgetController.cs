using BudgetTracker.DTO;
using BudgetTracker.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BudgetTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;

        public BudgetController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        [Authorize]
        [HttpPost("create-budget")]
        public async Task<IActionResult> CreateBudgetAsync(BudgetRequest request)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _budgetService.CreateBudgetAsync(userId.Value, request);
            if (response == null)
            {
                return Conflict("A budget already exists for this month.");
            }
            return Ok(response);
        }


        [Authorize]
        [HttpPatch("edit-budget/{month}")]
        public async Task<IActionResult> EditBudgetAsync( DateOnly month, BudgetRequest request)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _budgetService.EditBudgetAsync( userId.Value, month, request);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }


        [Authorize]
        [HttpGet("get-budget/{month}")]
        public async Task<IActionResult> GetBudgetAsync(DateOnly month)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _budgetService.GetBudgetAsync( userId.Value, month);

            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }


        [Authorize]
        [HttpGet("get-all-budgets")]
        public async Task<IActionResult> GetAllBudgetsAsync()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _budgetService.GetAllBudgetsAsync(
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
