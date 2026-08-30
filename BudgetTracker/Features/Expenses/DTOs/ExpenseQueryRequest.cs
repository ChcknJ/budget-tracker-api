using BudgetTracker.Features.Expenses.Models;

namespace BudgetTracker.Features.Expenses.DTOs
{
    public record ExpenseQueryRequest(
        Guid? CategoryId,
        Guid? SubscriptionId,
        DateOnly? FromDate,
        DateOnly? ToDate,
        decimal? MinimumAmount,
        decimal? MaximumAmount,
        int PageNumber,
        int PageSize,
        SortBy? SortByRequest,
        SortOrder? SortingOrder);
}
