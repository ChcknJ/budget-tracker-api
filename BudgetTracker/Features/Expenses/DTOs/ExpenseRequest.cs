namespace BudgetTracker.Features.Expenses.DTOs
{
    public record ExpenseRequest(
        Guid CategoryId,
        Guid? SubscriptionId,
        decimal Amount,
        string? Description,
        DateOnly Date);
}
