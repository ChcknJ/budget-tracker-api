using BudgetTracker.Features.Expenses.Models;
using Microsoft.OpenApi.MicrosoftExtensions;

namespace BudgetTracker.Features.Expenses.DTOs
{
    public record ExpensePaginatedResult(
        int Page,
        int PageSize,
        int TotalItems,
        int TotalPages,
        List<ExpenseResponse> Items);
}
