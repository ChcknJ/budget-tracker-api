namespace BudgetTracker.Features.Summary.DTOs
{
    public record SummaryResponse(
        decimal TotalExpenses,
        int NumberOfExpenses,
        decimal AverageExpense,
        decimal LargestExpense,
        decimal? BudgetForTheMonth,
        decimal? RemainingBudget,
        List<CategoryBreakdownResponse> CategoryBreakdown);
}
