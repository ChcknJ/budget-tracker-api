using BudgetTracker.Features.Categories.Models;

namespace BudgetTracker.Features.Categories.DTOs
{
    public record CategoryResult(
        bool Success,
        CategoryErrorType? ErrorType,
        string? ErrorMessage,
        CategoryResponse? Response);
}
