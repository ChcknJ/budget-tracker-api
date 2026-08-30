namespace BudgetTracker.Features.Budgets.DTOs
{
    public record BudgetListResult(
        int TotalBudgets,
        List<BudgetResponse> Items);
}
