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
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [Authorize]
        [HttpPost("create-expense")]
        public async Task<IActionResult> CreateExpenseAsync (ExpenseRequest request)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _expenseService.CreateExpenseAsync(request, userId.Value);

            return Ok(response);
        }


        [Authorize]
        [HttpPatch("edit-expense/{expenseId}")]
        public async Task<IActionResult> UpdateExpenseAsync (ExpenseRequest request, Guid expenseId)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _expenseService.EditExpenseAsync(userId.Value, expenseId, request);
            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }


        [Authorize]
        [HttpGet("get-expenses")]
        public async Task<IActionResult> GetExpensesAsync (ExpenseFilterRequest filter)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _expenseService.GetExpensesAsync(userId.Value, filter);
            return Ok(response);
        }


        [Authorize]
        [HttpDelete("delete-expense/{expenseId}")]
        public async Task<IActionResult> DeleteExpenseAsync (Guid expenseId)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _expenseService.DeleteExpenseAsync(userId.Value, expenseId);
            if (!response)
            {
                return NotFound();
            }

            return NoContent();
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
