using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.RateLimiting;
using BudgetTracker.Features.Expenses.DTOs;
using BudgetTracker.Features.Expenses.Interfaces;

namespace BudgetTracker.Features.Expenses.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IWriteExpenseService _writExpenseService;
        private readonly IReadExpenseService _readExpenseService;

        public ExpenseController(IReadExpenseService readExpenseService, IWriteExpenseService writeExpenseService)
        {
            _writExpenseService = writeExpenseService;
            _readExpenseService = readExpenseService;
        }

        [Authorize]
        [EnableRateLimiting("General")]
        [HttpPost("create-expense")]
        public async Task<IActionResult> CreateExpenseAsync (ExpenseRequest request)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _writExpenseService.CreateExpenseAsync(userId.Value, request);

            return Ok(response);
        }


        [Authorize]
        [EnableRateLimiting("General")]
        [HttpPatch("edit-expense/{expenseId}")]
        public async Task<IActionResult> UpdateExpenseAsync (ExpenseRequest request, Guid expenseId)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _writExpenseService.EditExpenseAsync(userId.Value, expenseId, request);
            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }


        [Authorize]
        [EnableRateLimiting("General")]
        [HttpGet("get-expenses")]
        public async Task<IActionResult> GetExpensesAsync ([FromQuery] ExpenseQueryRequest query)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _readExpenseService.GetFilteredExpensesAsync(userId.Value, query);
            return Ok(response);
        }


        [Authorize]
        [EnableRateLimiting("General")]
        [HttpDelete("delete-expense/{expenseId}")]
        public async Task<IActionResult> DeleteExpenseAsync (Guid expenseId)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _writExpenseService.DeleteExpenseAsync(userId.Value, expenseId);
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
