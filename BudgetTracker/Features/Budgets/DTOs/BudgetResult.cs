using BudgetTracker.Features.Budgets.Models;

namespace BudgetTracker.Features.Budgets.DTOs
{
    public record BudgetResult(
        bool Success,
        BudgetErrorType? ErrorType,
        string? ErrorMessage,
        BudgetResponse? Response);
}
