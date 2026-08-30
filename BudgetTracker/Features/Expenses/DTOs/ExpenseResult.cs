using BudgetTracker.Features.Expenses.Models;

namespace BudgetTracker.Features.Expenses.DTOs
{
    public record ExpenseResult(
        bool Success,
        ExpenseErrorType? ErrorType,
        string? ErrorMessage,
        ExpenseResponse? Response);
}
