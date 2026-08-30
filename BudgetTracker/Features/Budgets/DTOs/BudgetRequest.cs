namespace BudgetTracker.Features.Budgets.DTOs
{
    public record BudgetRequest(
        DateOnly Month,
        decimal Amount);
}
