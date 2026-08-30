using BudgetTracker.Features.Expenses.DTOs;
using BudgetTracker.Features.Expenses.Interfaces;
using BudgetTracker.Features.Expenses.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace BudgetTracker.Features.Expenses.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IWriteExpenseService _writeExpenseService;
        private readonly IReadExpenseService _readExpenseService;

        public ExpenseController(IReadExpenseService readExpenseService, IWriteExpenseService writeExpenseService)
        {
            _writeExpenseService = writeExpenseService;
            _readExpenseService = readExpenseService;
        }



        [Authorize]
        [EnableRateLimiting("General")]
        [HttpPost("create-expense")]
        public async Task<IActionResult> CreateExpenseAsync (ExpenseRequest request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _writeExpenseService.CreateExpenseAsync(userId.Value, request, cancellationToken);

            return response.Success ? Created(string.Empty, response) : MapError(response);
        }


        [Authorize]
        [EnableRateLimiting("General")]
        [HttpPatch("edit-expense/{expenseId}")]
        public async Task<IActionResult> UpdateExpenseAsync (ExpenseRequest request, Guid expenseId, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _writeExpenseService.EditExpenseAsync(userId.Value, expenseId, request, cancellationToken);
            return response.Success ? Ok(response) : MapError(response);
        }


        [Authorize]
        [EnableRateLimiting("General")]
        [HttpGet("get-expenses")]
        public async Task<IActionResult> GetExpensesAsync ([FromQuery] ExpenseQueryRequest query, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _readExpenseService.GetFilteredExpensesAsync(userId.Value, query, cancellationToken);
            return Ok(response);
        }


        [Authorize]
        [EnableRateLimiting("General")]
        [HttpDelete("delete-expense/{expenseId}")]
        public async Task<IActionResult> DeleteExpenseAsync (Guid expenseId, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _writeExpenseService.DeleteExpenseAsync(userId.Value, expenseId, cancellationToken);
            return response.Success ? NoContent() : MapError(response);
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


        private IActionResult MapError(ExpenseResult result)
        {
            return result.ErrorType switch
            {
                ExpenseErrorType.InvalidCategory => BadRequest(result.ErrorMessage),
                ExpenseErrorType.InvalidSubscription => BadRequest(result.ErrorMessage),
                ExpenseErrorType.InvalidRequest => NotFound(result.ErrorMessage),
                ExpenseErrorType.ExpenseNotFound => NotFound(result.ErrorMessage),
                _ => BadRequest(result.ErrorMessage)
            };
        }
    }
}
