namespace BudgetTracker.Features.Budgets.DTOs
{
    public record BudgetResponse(
        Guid Id,
        DateOnly Month,
        decimal Amount);
}
